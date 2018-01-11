/* Set database and clear queued statements */
USE quiztool;
GO

/* Reset database - drop existing tables */
DROP TABLE IF EXISTS Answer;
DROP TABLE IF EXISTS Question;
DROP TABLE IF EXISTS ExamTopic;
DROP TABLE IF EXISTS Exam;
DROP TABLE IF EXISTS Topic;
DROP TABLE IF EXISTS Subject;
DROP TABLE IF EXISTS Teacher;

/*
	TABLE Teacher
*/
CREATE TABLE Teacher (
	Id					INT IDENTITY PRIMARY KEY,
	Name				VARCHAR(256) UNIQUE NOT NULL,
	Email				VARCHAR(256) NOT NULL,
	Password			VARCHAR(256) NOT NULL,
	PasswordResetCode	VARCHAR(64) DEFAULT NULL
);

/*
	TABLE Subject
*/
CREATE TABLE Subject (
	Id					INT IDENTITY PRIMARY KEY,
	Name				VARCHAR(256) UNIQUE NOT NULL
);

/*
	TABLE Topic
*/
CREATE TABLE Topic (
	Id					INT IDENTITY PRIMARY KEY,
	Name				VARCHAR(256) NOT NULL,
	SubjectId			INT NOT NULL,
	
	CONSTRAINT FK_Topic_Subject
		FOREIGN KEY (SubjectId)
		REFERENCES Subject(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
);

/*
	TABLE Exam
*/
CREATE TABLE Exam (
	Id					INT IDENTITY PRIMARY KEY,
	Name				VARCHAR(256) NOT NULL,
	Timelimit			SMALLINT,
	Minimumscore		SMALLINT,
	GenerateMethod		TINYINT NOT NULL,
	GenerateInput		SMALLINT NOT NULL,
	MinimumTopicsPassed	TINYINT,
	IsHidden			BIT NOT NULL,
	SubjectId			INT NOT NULL,
	
	CONSTRAINT FK_Exam_Subject
		FOREIGN KEY (SubjectId)
		REFERENCES Subject(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION,
	
	CONSTRAINT U_Exam_NameAndSubject
		UNIQUE (SubjectId, Name),
	
	CONSTRAINT CHK_Exam_GenerateMethodValues
		CHECK (GenerateMethod IN (0, 1)),
	
	CONSTRAINT CHK_Exam_PassRequirements
		CHECK (MinimumScore IS NOT NULL OR MinimumTopicsPassed IS NOT NULL)
);

/*
	TABLE ExamTopic
*/
CREATE TABLE ExamTopic (
	ExamId				INT NOT NULL,
	TopicId				INT NOT NULL,
	NumberOfQuestions	SMALLINT,
	MinimumScore		SMALLINT,
	
	CONSTRAINT PK_ExamTopic
		PRIMARY KEY(ExamId, TopicId),
	
	CONSTRAINT FK_ExamTopic_ExamId
		FOREIGN KEY (ExamId)
		REFERENCES Exam(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	
	CONSTRAINT FK_ExamTopic_TopicId
		FOREIGN KEY (TopicId)
		REFERENCES Topic(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

/*
	TABLE Question
*/
CREATE TABLE Question (
	Id					INT IDENTITY PRIMARY KEY,
	QuestionText		TEXT NOT NULL,
	Codeblock			TEXT,
	QuestionType		TINYINT NOT NULL,
	Weight				TINYINT NOT NULL DEFAULT 1,
	TopicId				INT NOT NULL,
	
	CONSTRAINT FK_Question_TopicId
		FOREIGN KEY (TopicId)
		REFERENCES Topic(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	
	CONSTRAINT CHK_Question_QuestionTypeValues
		CHECK (QuestionType IN (0, 1))
);

/*
	TABLE Answer
*/
CREATE TABLE Answer (
	QuestionId			INT NOT NULL,
	AnswerId			TINYINT NOT NULL,
	AnswerText			VARCHAR(128) NOT NULL,
	IsCorrect			BIT NOT NULL,
	
	CONSTRAINT PK_Answer
		PRIMARY KEY (QuestionId, AnswerId),
	
	CONSTRAINT FK_Answer_QuestionId
		FOREIGN KEY (QuestionId)
		REFERENCES Question(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	
	CONSTRAINT U_Answer_UniqueAnswer
		UNIQUE(QuestionId, AnswerText)
);

/*
	Send statements to server for execution
	Executed before creating triggers, because creating a trigger must be the first statement in a batch.
*/
GO

/*
	TRIGGER Delete_Subject
	The database schema has multiple cascade paths from Subject to ExamTopic (through Exam and through Topic).
	MS SQL Server however doesn't allow multiple cascade paths.
	Therefore a trigger is used to declare the deletion logic for the Subject table, instead of a cascade.
*/
CREATE TRIGGER Delete_Subject
   ON Subject
   INSTEAD OF DELETE
AS
BEGIN
 SET NOCOUNT ON;
 DELETE FROM Exam WHERE SubjectId IN (SELECT Id FROM DELETED)
 DELETE FROM Topic WHERE SubjectId IN (SELECT Id FROM DELETED)
 DELETE FROM Subject WHERE Id IN (SELECT Id FROM DELETED)
END;

GO

/*
	TRIGGER Delete_Topic
	An exam needs at least 1 topic. If the last topic of an exam gets deleted, delete the exam.
*/
CREATE TRIGGER Delete_Topic
   ON Topic
   AFTER DELETE
AS
BEGIN
 SET NOCOUNT ON;
 DELETE FROM Subject WHERE Id IN (SELECT SubjectId FROM DELETED) AND Id NOT IN (SELECT SubjectId FROM Topic);
END;

GO

/*
	TRIGGER Delete_ExamTopic
	An exam needs at least 1 topic. If the last topic of an exam gets deleted, delete the exam.
*/
CREATE TRIGGER Delete_ExamTopic
   ON ExamTopic
   AFTER DELETE
AS
BEGIN
 SET NOCOUNT ON;
 DELETE FROM ExamTopic WHERE ExamId IN (SELECT ExamId FROM DELETED) AND TopicId IN (SELECT TopicId FROM DELETED);
 DELETE FROM Exam WHERE Id IN (SELECT ExamId FROM DELETED) AND Id NOT IN (SELECT ExamId FROM ExamTopic);
END;

GO