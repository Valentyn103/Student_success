--create database student_successdb
use student_successdb

create table Groups
(
	Id int identity primary key
	, Name nvarchar(64)
	, CreatedDate datetime
)

create table Student
(
	Id int identity primary key
	, GroupsId int not null references Groups(Id)
	, Name nvarchar(64)
	, Surname nvarchar(64)
	, Email nvarchar(64)
	, Number nvarchar(64)
)
create table Subjects
(
	Id int identity primary key
	, Name nvarchar(64)
)
create table Groups_Subjects
(
	Id int identity primary key
	, GroupsId int not null references Groups(Id)
	, SubjectsId int not null references Subjects(Id)
)
create table Marks
(
	Id int identity primary key
	, StudentId int not null references Student(Id)
	, SubjectsId int not null references Subjects(Id)
	, MarkDate datetime
	, Mark int
)
create table Messages
(
	Id int identity primary key
	, StudentId int not null references Student(Id)
)

select * from Groups
select * from Student
select * from Subjects
select * from Groups_Subjects
select * from Marks
select * from Messages

drop table Messages
drop table Marks
drop table Groups_Subjects
drop table Subjects
drop table Student
drop table Groups

