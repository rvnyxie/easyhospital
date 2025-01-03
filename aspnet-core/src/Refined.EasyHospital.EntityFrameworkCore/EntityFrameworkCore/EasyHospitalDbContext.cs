﻿using Microsoft.EntityFrameworkCore;
using Refined.EasyHospital.Communes;
using Refined.EasyHospital.Districts;
using Refined.EasyHospital.Hospitals;
using Refined.EasyHospital.Patients;
using Refined.EasyHospital.Provinces;
using Refined.EasyHospital.UserHospitals;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Refined.EasyHospital.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class EasyHospitalDbContext :
    AbpDbContext<EasyHospitalDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    #region Development Entities

    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Commune> Communes { get; set; }

    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<UserHospital> UserHospitals { get; set; }
    public DbSet<Patient> Patients { get; set; }

    #endregion

    public EasyHospitalDbContext(DbContextOptions<EasyHospitalDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        // Province
        builder.Entity<Province>(b =>
        {
            b.ToTable(EasyHospitalConsts.DbTablePrefix + "Provinces", EasyHospitalConsts.DbSchema);
            b.ConfigureByConvention();

            //Property constraints
            b.Property(p => p.Code).IsRequired().HasMaxLength(2);
            b.Property(p => p.Name).IsRequired().HasMaxLength(100);
            b.Property(p => p.DecisionDate);
            b.Property(p => p.EffectiveDate);
            b.Property(p => p.Population);
            b.Property(p => p.Area);
            b.Property(p => p.Description).HasMaxLength(512);

            b.HasIndex(p => p.Code).IsUnique();
            b.HasIndex(p => p.Name).IsUnique();
        });

        // District
        builder.Entity<District>(b =>
        {
            b.ToTable(EasyHospitalConsts.DbTablePrefix + "Districts", EasyHospitalConsts.DbSchema);
            b.ConfigureByConvention();

            // Property constraints
            b.Property(p => p.Code).IsRequired().HasMaxLength(3);
            b.Property(p => p.Name).IsRequired().HasMaxLength(100);
            b.Property(p => p.DecisionDate);
            b.Property(p => p.EffectiveDate);
            b.Property(p => p.Population);
            b.Property(p => p.Area);
            b.Property(p => p.Description).HasMaxLength(512);
            b.Property(p => p.ProvinceCode).HasMaxLength(50);

            b.HasIndex(p => p.Code).IsUnique();
        });

        // Commune
        builder.Entity<Commune>(b =>
        {
            b.ToTable(EasyHospitalConsts.DbTablePrefix + "Communes", EasyHospitalConsts.DbSchema);
            b.ConfigureByConvention();

            // Property constraints
            b.Property(p => p.Code).IsRequired().HasMaxLength(5);
            b.Property(p => p.Name).IsRequired().HasMaxLength(100);
            b.Property(p => p.DecisionDate);
            b.Property(p => p.EffectiveDate);
            b.Property(p => p.Population);
            b.Property(p => p.Area);
            b.Property(p => p.Description).HasMaxLength(512);
            b.Property(p => p.DistrictCode).HasMaxLength(50);

            b.HasIndex(p => p.Code).IsUnique();
        });

        // Hospital
        builder.Entity<Hospital>(b =>
        {
            b.ToTable(EasyHospitalConsts.DbTablePrefix + "Hospitals", EasyHospitalConsts.DbSchema);
            b.ConfigureByConvention();

            // Property constraints
            b.Property(p => p.Name).IsRequired().HasMaxLength(100);
            b.Property(p => p.ProvinceCode).IsRequired().HasMaxLength(50);
            b.Property(p => p.DistrictCode).IsRequired().HasMaxLength(50);
            b.Property(p => p.CommuneCode).IsRequired().HasMaxLength(50);

            b.HasIndex(p => new { p.ProvinceCode, p.DistrictCode, p.CommuneCode });
        });

        // UserHospital
        builder.Entity<UserHospital>(b =>
        {
            b.ToTable(EasyHospitalConsts.DbTablePrefix + "UserHospitals", EasyHospitalConsts.DbSchema);
            b.ConfigureByConvention();

            // Propery constraints
            b.Property(p => p.UserId).IsRequired();
            b.Property(p => p.HospitalId).IsRequired();
        });

        // Patient
        builder.Entity<Patient>(b =>
        {
            b.ToTable(EasyHospitalConsts.DbTablePrefix + "Patients", EasyHospitalConsts.DbSchema);
            b.ConfigureByConvention();

            // Property constraints
            b.Property(p => p.Code).IsRequired().HasMaxLength(50);
            b.Property(p => p.Name).IsRequired().HasMaxLength(100);
            b.Property(p => p.ProvinceCode).IsRequired().HasMaxLength(50);
            b.Property(p => p.DistrictCode).IsRequired().HasMaxLength(50);
            b.Property(p => p.CommuneCode).IsRequired().HasMaxLength(50);
            b.Property(p => p.HospitalId).IsRequired();

            b.HasIndex(p => p.Code).IsUnique();
        });
    }
}
