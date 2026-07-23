UPDATE Books
SET AvailableQuantity = AvailableQuantity - 1
WHERE BookId = 1;

ALTER TABLE Books
ADD Price DECIMAL(10,2) NOT NULL DEFAULT 0;

UPDATE Books
SET Price =
CASE BookId
WHEN 1 THEN 450000
WHEN 2 THEN 650000
WHEN 3 THEN 550000
WHEN 4 THEN 600000
WHEN 5 THEN 700000
END;

-- Xóa cột Gender kiểu NVARCHAR cũ
ALTER TABLE Members
DROP COLUMN Gender;
GO

-- Tạo lại Gender kiểu BIT giống thiết kế cũ
ALTER TABLE Members
ADD Gender BIT NULL;
GO

UPDATE Members
SET Gender = 1
WHERE MemberId = 1;
GO

UPDATE Members
SET Gender = 0
WHERE MemberId = 2;
GO

UPDATE Members
SET Gender = 1
WHERE MemberId = 3;
GO