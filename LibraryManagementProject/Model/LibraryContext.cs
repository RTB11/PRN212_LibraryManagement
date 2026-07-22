using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementProject.Model;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }


    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowDetail> BorrowDetails { get; set; }

    public virtual DbSet<BorrowRecord> BorrowRecords { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }



    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=LibraryManagement;User Id=sa;Password=123;TrustServerCertificate=True;");
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // =========================
        // AUTHOR
        // =========================

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId);

            entity.Property(e => e.AuthorName)
                .HasMaxLength(100);
        });



        // =========================
        // BOOK
        // =========================

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId);


            entity.HasIndex(e => e.Isbn)
                .IsUnique();


            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");


            entity.Property(e => e.Title)
                .HasMaxLength(200);


            entity.Property(e => e.Publisher)
                .HasMaxLength(100);


            entity.Property(e => e.Shelf)
                .HasMaxLength(50);


            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255);


            entity.Property(e => e.Status)
                .HasDefaultValue(true);



            entity.HasOne(e => e.Author)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            entity.HasOne(e => e.Category)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });




        // =========================
        // BORROW DETAIL
        // =========================

        modelBuilder.Entity<BorrowDetail>(entity =>
        {

            entity.HasKey(e => e.BorrowDetailId);



            entity.Property(e => e.FineAmount)
        .HasColumnType("decimal(10,2)")
        .HasDefaultValue(0m);



            entity.Property(e => e.Quantity)
                .HasDefaultValue(1);



            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Borrowing");



            entity.HasOne(e => e.Book)
                .WithMany(e => e.BorrowDetails)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            entity.HasOne(e => e.Borrow)
                .WithMany(e => e.BorrowDetails)
                .HasForeignKey(e => e.BorrowId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });






        // =========================
        // BORROW RECORD
        // =========================

        modelBuilder.Entity<BorrowRecord>(entity =>
        {

            entity.HasKey(e => e.BorrowId);



            entity.HasIndex(e => e.BorrowCode)
                .IsUnique();



            entity.Property(e => e.BorrowCode)
                .HasMaxLength(20);



            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())");



            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");



            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Borrowing");



            entity.HasOne(e => e.Member)
                .WithMany(e => e.BorrowRecords)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            entity.HasOne(e => e.User)
                .WithMany(e => e.BorrowRecords)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });






        // =========================
        // CATEGORY
        // =========================

        modelBuilder.Entity<Category>(entity =>
        {

            entity.HasKey(e => e.CategoryId);


            entity.HasIndex(e => e.CategoryName)
                .IsUnique();


            entity.Property(e => e.CategoryName)
                .HasMaxLength(100);


            entity.Property(e => e.Description)
                .HasMaxLength(255);

        });






        // =========================
        // MEMBER
        // =========================

        modelBuilder.Entity<Member>(entity =>
        {

            entity.HasKey(e => e.MemberId);


            entity.Property(e => e.FullName)
                .HasMaxLength(100);


            entity.Property(e => e.Email)
                .HasMaxLength(100);


            entity.Property(e => e.Phone)
                .HasMaxLength(20);


            entity.Property(e => e.Address)
                .HasMaxLength(255);


            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())");


            entity.Property(e => e.Status)
                .HasDefaultValue(true);

        });







        // =========================
        // ROLE
        // =========================

        modelBuilder.Entity<Role>(entity =>
        {

            entity.HasKey(e => e.RoleId);


            entity.HasIndex(e => e.RoleName)
                .IsUnique();


            entity.Property(e => e.RoleName)
                .HasMaxLength(50);

        });








        // =========================
        // USER
        // =========================

        modelBuilder.Entity<User>(entity =>
        {

            entity.HasKey(e => e.UserId);



            entity.HasIndex(e => e.Username)
                .IsUnique();



            entity.Property(e => e.Username)
                .HasMaxLength(50);



            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255);



            entity.Property(e => e.FullName)
                .HasMaxLength(100);



            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");



            entity.Property(e => e.Status)
                .HasDefaultValue(true);




            entity.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });



        OnModelCreatingPartial(modelBuilder);

    }



    partial void OnModelCreatingPartial(
        ModelBuilder modelBuilder);
}