-- ==========================================
-- CREATE DATABASE
-- ==========================================

CREATE DATABASE LibraryManagement;
GO

USE LibraryManagement;
GO

/*======================================================
    ROLES
======================================================*/
CREATE TABLE Roles
(
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO Roles(RoleName)
VALUES
('Admin'),
('Librarian');


/*======================================================
    USERS
======================================================*/
CREATE TABLE Users
(
    UserId INT IDENTITY PRIMARY KEY,

    Username NVARCHAR(50) NOT NULL UNIQUE,

    PasswordHash NVARCHAR(255) NOT NULL,

    FullName NVARCHAR(100) NOT NULL,

    RoleId INT NOT NULL,

    Status BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Users_Roles
        FOREIGN KEY(RoleId)
        REFERENCES Roles(RoleId)
);
/*======================================================
    AUTHORS
======================================================*/
CREATE TABLE Authors
(
    AuthorId INT IDENTITY PRIMARY KEY,
    AuthorName NVARCHAR(100) NOT NULL,
    Biography NVARCHAR(MAX)
);



/*======================================================
    CATEGORIES
======================================================*/
CREATE TABLE Categories
(
    CategoryId INT IDENTITY PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);



/*======================================================
    MEMBERS
======================================================*/
CREATE TABLE Members
(
    MemberId INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    JoinDate DATE DEFAULT GETDATE(),
    Status BIT DEFAULT 1
);



/*======================================================
    BOOKS
======================================================*/
CREATE TABLE Books
(
    BookId INT IDENTITY PRIMARY KEY,

    Title NVARCHAR(200) NOT NULL,

    ISBN NVARCHAR(20) UNIQUE,

    AuthorId INT NOT NULL,

    CategoryId INT NOT NULL,

    Publisher NVARCHAR(100),

    PublishYear INT,

    Quantity INT NOT NULL CHECK(Quantity>=0),

    AvailableQuantity INT NOT NULL CHECK(AvailableQuantity>=0),

    Shelf NVARCHAR(50),

    ImageUrl NVARCHAR(255),

    Status BIT DEFAULT 1,

    CONSTRAINT FK_Books_Authors
        FOREIGN KEY(AuthorId)
        REFERENCES Authors(AuthorId),

    CONSTRAINT FK_Books_Categories
        FOREIGN KEY(CategoryId)
        REFERENCES Categories(CategoryId)
);



/*======================================================
    BORROW RECORDS
======================================================*/
CREATE TABLE BorrowRecords
(
    BorrowId INT IDENTITY PRIMARY KEY,

    BorrowCode NVARCHAR(20) UNIQUE,

    MemberId INT NOT NULL,

    BorrowDate DATE NOT NULL DEFAULT GETDATE(),

    DueDate DATE NOT NULL,

    Status NVARCHAR(20) NOT NULL DEFAULT 'Borrowing',

    UserId INT NOT NULL,

    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_BorrowRecord_Member
        FOREIGN KEY(MemberId)
        REFERENCES Members(MemberId),

    CONSTRAINT FK_BorrowRecord_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);



/*======================================================
    BORROW DETAILS
======================================================*/
CREATE TABLE BorrowDetails
(
    BorrowDetailId INT IDENTITY PRIMARY KEY,

    BorrowId INT NOT NULL,

    BookId INT NOT NULL,

    Quantity INT NOT NULL DEFAULT 1
        CHECK(Quantity>0),

    --------------------------------------------------
    -- Borrow information
    --------------------------------------------------

    BorrowCondition NVARCHAR(30),

    BorrowNote NVARCHAR(255),

    --------------------------------------------------
    -- Return information
    --------------------------------------------------

    ReturnDate DATE,

    ReturnCondition NVARCHAR(30),

    ReturnNote NVARCHAR(255),

    --------------------------------------------------
    -- Fine
    --------------------------------------------------

    FineAmount DECIMAL(10,2) DEFAULT 0,

    Status NVARCHAR(20) DEFAULT 'Borrowing',

    CONSTRAINT FK_BorrowDetail_Record
        FOREIGN KEY(BorrowId)
        REFERENCES BorrowRecords(BorrowId),

    CONSTRAINT FK_BorrowDetail_Book
        FOREIGN KEY(BookId)
        REFERENCES Books(BookId),

    CONSTRAINT CK_BorrowCondition
        CHECK
        (
            BorrowCondition IS NULL
            OR BorrowCondition IN
            (
                'Good',
                'Fair',
                'Damaged',
                'Lost'
            )
        ),

    CONSTRAINT CK_ReturnCondition
        CHECK
        (
            ReturnCondition IS NULL
            OR ReturnCondition IN
            (
                'Good',
                'Fair',
                'Damaged',
                'Lost'
            )
        )
);

-- ==========================================
-- SAMPLE USERS
-- Password: 123456
-- (Hash sau này sẽ thay bằng ASP.NET Identity hoặc BCrypt)
-- ==========================================
INSERT INTO Users
(Username,PasswordHash,FullName,RoleId)
VALUES
('admin','123456','Administrator',1),
('librarian','123456','Library Staff',2);

-- ==========================================
-- AUTHORS
-- ==========================================

INSERT INTO Authors
(AuthorName)
VALUES
('Robert C. Martin'),
('Martin Fowler'),
('Joshua Bloch'),
('Andrew Hunt'),
('Erich Gamma');

-- ==========================================
-- CATEGORIES
-- ==========================================

INSERT INTO Categories
(CategoryName)
VALUES
('Programming'),
('Database'),
('Software Engineering'),
('Web Development');

-- ==========================================
-- MEMBERS
-- ==========================================

INSERT INTO Members
(FullName,Email,Phone,Address)
VALUES
('Nguyen Van A','a@gmail.com','0900000001','Ha Noi'),
('Tran Thi B','b@gmail.com','0900000002','Da Nang'),
('Le Van C','c@gmail.com','0900000003','Ho Chi Minh');

-- ==========================================
-- BOOKS
-- ==========================================

INSERT INTO Books
(Title,ISBN,AuthorId,CategoryId,Publisher,PublishYear,Quantity,AvailableQuantity,Shelf)
VALUES

('Clean Code','9780132350884',
1,
1,
'Prentice Hall',
2008,
10,
10,
'A01'
),

(
'Refactoring',
'9780134757599',
2,
3,
'Addison Wesley',
2018,
8,
8,
'A02'
),

(
'Effective Java',
'9780134685991',
3,
1,
'Addison Wesley',
2018,
6,
6,
'A03'
),

(
'The Pragmatic Programmer',
'9780135957059',
4,
3,
'Addison Wesley',
2019,
5,
5,
'A04'
),

(
'Design Patterns',
'9780201633610',
5,
3,
'Addison Wesley',
1994,
4,
4,
'A05'
);

-- ==========================================
-- SAMPLE BORROW
-- ==========================================

INSERT INTO BorrowRecords
(
    BorrowCode,
    MemberId,
    BorrowDate,
    DueDate,
    Status,
    UserId
)
VALUES
(
    'BR001',
    1,
    GETDATE(),
    DATEADD(DAY,14,GETDATE()),
    'Borrowing',
    2
);


DECLARE @BorrowId INT = SCOPE_IDENTITY();


INSERT INTO BorrowDetails
(
    BorrowId,
    BookId,
    Quantity,
    BorrowCondition,
    BorrowNote,
    Status
)
VALUES
(
    @BorrowId,
    1,
    1,
    'Good',
    'Book cover slightly old',
    'Borrowing'
);


UPDATE Books
SET AvailableQuantity = AvailableQuantity - 1
WHERE BookId = 1;