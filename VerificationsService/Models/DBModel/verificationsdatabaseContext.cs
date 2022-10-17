using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace VerificationsService.Models.DBModel
{
    public partial class verificationsdatabaseContext : DbContext
    {
        public verificationsdatabaseContext(DbContextOptions<verificationsdatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Challenge> Challenges { get; set; }
        public virtual DbSet<SmsChallenge> SmsChallenges { get; set; }
        public virtual DbSet<Verification> Verifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.ToTable("challenges");

                entity.Property(e => e.ChallengeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("challenge_id");

                entity.Property(e => e.Channel)
                    .HasColumnType("enum('sms','mail','push_notification')")
                    .HasColumnName("channel");

                entity.Property(e => e.Code)
                    .HasMaxLength(64)
                    .HasColumnName("code")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnType("datetime")
                    .HasColumnName("expires_at");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('pending','approved','denied')")
                    .HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasColumnType("enum('otp')")
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<SmsChallenge>(entity =>
            {
                entity.ToTable("sms_challenges");

                entity.Property(e => e.SmsChallengeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sms_challenge_id");

                entity.Property(e => e.ChallengeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("challenge_id");

                entity.Property(e => e.ReceiverNumber)
                    .HasMaxLength(20)
                    .HasColumnName("receiver_number");

                entity.HasOne(d => d.Challenge)
                    .WithMany(p => p.SmsChallenges)
                    .HasForeignKey(d => d.ChallengeId)
                    .HasConstraintName("sms_challenges_ibfk_1");
            });

            modelBuilder.Entity<Verification>(entity =>
            {
                entity.ToTable("verifications");

                entity.Property(e => e.VerificationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("verificationId");

                entity.Property(e => e.Attempt)
                    .HasColumnType("enum('successful','failure')")
                    .HasColumnName("attempt");

                entity.Property(e => e.ChallengeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("challenge_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at");

                entity.HasOne(d => d.Challenge)
                    .WithMany(p => p.Verifications)
                    .HasForeignKey(d => d.ChallengeId)
                    .HasConstraintName("verifications_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
