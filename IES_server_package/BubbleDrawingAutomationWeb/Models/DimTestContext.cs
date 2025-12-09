using System;
using System.Collections.Generic;
using BubbleDrawingAutomationWeb.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BubbleDrawingAutomationWeb.Models;

public partial class DimTestContext : DbContext
{
    private readonly AppSettings _appSettings;
    public DimTestContext(IOptions<AppSettings> opt)
    {
        _appSettings = opt.Value;
    }

    public DimTestContext(DbContextOptions<DimTestContext> options, IOptions<AppSettings> opt)
        : base(options)
    {
        _appSettings = opt.Value;
    }

    public virtual DbSet<AppSecurity> AppSecurities { get; set; }

    public virtual DbSet<AppSrvrDbSrvr> AppSrvrDbSrvrs { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DimsActual> DimsActuals { get; set; }

    public virtual DbSet<DimsActualAudit> DimsActualAudits { get; set; }

    public virtual DbSet<Drawing> Drawings { get; set; }

    public virtual DbSet<DwgDim> DwgDims { get; set; }

    public virtual DbSet<DwgFile> DwgFiles { get; set; }

    public virtual DbSet<Empl> Empls { get; set; }

    public virtual DbSet<Gage> Gages { get; set; }

    public virtual DbSet<MpmSrvr> MpmSrvrs { get; set; }

    public virtual DbSet<Mtrl> Mtrls { get; set; }

    public virtual DbSet<PartSn> PartSns { get; set; }

    public virtual DbSet<Plnt> Plnts { get; set; }

    public virtual DbSet<PlntMpm> PlntMpms { get; set; }

    public virtual DbSet<PrchOrdr> PrchOrdrs { get; set; }

    public virtual DbSet<PrchOrdrItm> PrchOrdrItms { get; set; }

    public virtual DbSet<ProdOrdr> ProdOrdrs { get; set; }

    public virtual DbSet<Sqlbulkinsert> Sqlbulkinserts { get; set; }

    public virtual DbSet<SrvrShr> SrvrShrs { get; set; }

    public virtual DbSet<TblAuditHistory> TblAuditHistories { get; set; }

    public virtual DbSet<TblBaloonDrawingHeader> TblBaloonDrawingHeaders { get; set; }

    public virtual DbSet<TblBaloonDrawingLiner> TblBaloonDrawingLiners { get; set; }

    public virtual DbSet<TblComment> TblComments { get; set; }

    public virtual DbSet<TblConfiguration> TblConfigurations { get; set; }

    public virtual DbSet<TblConfigurationBkp> TblConfigurationBkps { get; set; }

    public virtual DbSet<TblDbconfiguration> TblDbconfigurations { get; set; }

    public virtual DbSet<TblDimensionInputHeader> TblDimensionInputHeaders { get; set; }

    public virtual DbSet<TblDimensionInputHistory> TblDimensionInputHistories { get; set; }

    public virtual DbSet<TblDimensionInputLiner> TblDimensionInputLiners { get; set; }

    public virtual DbSet<TblMeasureSubType> TblMeasureSubTypes { get; set; }

    public virtual DbSet<TblMeasureType> TblMeasureTypes { get; set; }

    public virtual DbSet<TblOriginalBalloonedImageDetail> TblOriginalBalloonedImageDetails { get; set; }

    public virtual DbSet<TblUom> TblUoms { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserAccess> TblUserAccesses { get; set; }

    public virtual DbSet<TblWorkCenter> TblWorkCenters { get; set; }

    public virtual DbSet<XDimType> XDimTypes { get; set; }

    public virtual DbSet<XSrvrType> XSrvrTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(_appSettings.MPMConnStr);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSecurity>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("APP_SECURITY");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.BalloonPrnts).HasColumnName("BALLOON_PRNTS");
            entity.Property(e => e.ChangeDims).HasColumnName("CHANGE_DIMS");
            entity.Property(e => e.ChkOutDwgs).HasColumnName("CHK_OUT_DWGS");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("CRTMS");
            entity.Property(e => e.EmplOid).HasColumnName("EMPL_OID");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.RecordDims).HasColumnName("RECORD_DIMS");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.EmplO).WithMany(p => p.AppSecurities)
                .HasForeignKey(d => d.EmplOid)
                .HasConstraintName("FK_APP_SECURITY_EMPL");
        });

        modelBuilder.Entity<AppSrvrDbSrvr>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("APP_SRVR_DB_SRVR_PK");

            entity.ToTable("APP_SRVR_DB_SRVR");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg).HasColumnName("ACTV_FLG");
            entity.Property(e => e.AppSrvrNm)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APP_SRVR_NM");
            entity.Property(e => e.Crtms)
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DbNm)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DB_NM");
            entity.Property(e => e.DbSrvr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DB_SRVR");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.SrvrTypeCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SRVR_TYPE_CD");
            entity.Property(e => e.SrvrTypeOid).HasColumnName("SRVR_TYPE_OID");
            entity.Property(e => e.Updtms)
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.SrvrTypeO).WithMany(p => p.AppSrvrDbSrvrs)
                .HasForeignKey(d => d.SrvrTypeOid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("X_SRVR_TYPE_APP_SRVR_DB_SRVR_FK1");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("COMMENTS");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Comment1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("COMMENT");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DimsActualOid).HasColumnName("DIMS_ACTUAL_OID");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.DimsActualO).WithMany(p => p.Comments)
                .HasForeignKey(d => d.DimsActualOid)
                .HasConstraintName("FK_COMMENTS_DIMS_ACTUAL");
        });

        modelBuilder.Entity<DimsActual>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("DIMS_ACTUAL_PK");

            entity.ToTable("DIMS_ACTUAL");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.BalloonNbr).HasColumnName("BALLOON_NBR");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DimActual)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("DIM_ACTUAL");
            entity.Property(e => e.DimPass).HasColumnName("DIM_PASS");
            entity.Property(e => e.EmplOid).HasColumnName("EMPL_OID");
            entity.Property(e => e.LstUpdtBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPDT_BY");
            entity.Property(e => e.PartSnOid).HasColumnName("PART_SN_OID");
            entity.Property(e => e.UomOid).HasColumnName("UOM_OID");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.PartSnO).WithMany(p => p.DimsActuals)
                .HasForeignKey(d => d.PartSnOid)
                .HasConstraintName("FK_DIMS_ACTUAL_PART_SN");
        });

        modelBuilder.Entity<DimsActualAudit>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("DIMS_ACTUAL_AUDIT_PK");

            entity.ToTable("DIMS_ACTUAL_AUDIT");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.BalloonNbr).HasColumnName("BALLOON_NBR");
            entity.Property(e => e.ChangedBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CHANGED_BY");
            entity.Property(e => e.ChangedDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CHANGED_DT");
            entity.Property(e => e.Crtms)
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DimActual)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("DIM_ACTUAL");
            entity.Property(e => e.DimPass).HasColumnName("DIM_PASS");
            entity.Property(e => e.DimsActualOid).HasColumnName("DIMS_ACTUAL_OID");
            entity.Property(e => e.DrawingOid).HasColumnName("DRAWING_OID");
            entity.Property(e => e.EmplOid).HasColumnName("EMPL_OID");
            entity.Property(e => e.LstUpdtBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPDT_BY");
            entity.Property(e => e.PartSerialNbrOid).HasColumnName("PART_SERIAL_NBR_OID");
            entity.Property(e => e.UomOid).HasColumnName("UOM_OID");
            entity.Property(e => e.Updtms)
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.DimsActualO).WithMany(p => p.DimsActualAudits)
                .HasForeignKey(d => d.DimsActualOid)
                .HasConstraintName("FK_DIMS_ACTUAL_AUDIT_DIMS_ACTUAL");
        });

        modelBuilder.Entity<Drawing>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("DRAWING_PK");

            entity.ToTable("DRAWING");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DwgNbr)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("DWG_NBR");
            entity.Property(e => e.DwgRev)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("DWG_REV");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.MtrlOid).HasColumnName("MTRL_OID");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.MtrlO).WithMany(p => p.Drawings)
                .HasForeignKey(d => d.MtrlOid)
                .HasConstraintName("FK_DRAWING_MTRL");
        });

        modelBuilder.Entity<DwgDim>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("DWG_DIMS");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.BalloonNbr).HasColumnName("BALLOON_NBR");
            entity.Property(e => e.DimMax)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("DIM_MAX");
            entity.Property(e => e.DimMin)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("DIM_MIN");
            entity.Property(e => e.DimNominal)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("DIM_NOMINAL");
            entity.Property(e => e.DimTypeOid).HasColumnName("DIM_TYPE_OID");
            entity.Property(e => e.DrawingOid).HasColumnName("DRAWING_OID");
            entity.Property(e => e.UomOid).HasColumnName("UOM_OID");

            entity.HasOne(d => d.DimTypeO).WithMany(p => p.DwgDims)
                .HasForeignKey(d => d.DimTypeOid)
                .HasConstraintName("DIM_TYPE_DWG_DIMS_FK1");

            entity.HasOne(d => d.DrawingO).WithMany(p => p.DwgDims)
                .HasForeignKey(d => d.DrawingOid)
                .HasConstraintName("DRAWING_DWG_DIMS_FK1");

            entity.HasOne(d => d.UomO).WithMany(p => p.DwgDims)
                .HasForeignKey(d => d.UomOid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DWG_DIMS_X_UOM");
        });

        modelBuilder.Entity<DwgFile>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("DWG_FILE_PK");

            entity.ToTable("DWG_FILE");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DltFlg)
                .HasDefaultValueSql("((0))")
                .HasColumnName("DLT_FLG");
            entity.Property(e => e.DrawingOid).HasColumnName("DRAWING_OID");
            entity.Property(e => e.DwgFileNm)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DWG_FILE_NM");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.SrvrShrOid).HasColumnName("SRVR_SHR_OID");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.DrawingO).WithMany(p => p.DwgFiles)
                .HasForeignKey(d => d.DrawingOid)
                .HasConstraintName("DRAWING_DWG_FILE_FK1");

            entity.HasOne(d => d.SrvrShrO).WithMany(p => p.DwgFiles)
                .HasForeignKey(d => d.SrvrShrOid)
                .HasConstraintName("SRVR_SHR_DWG_FILE_FK1");
        });

        modelBuilder.Entity<Empl>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("EMPL_PK");

            entity.ToTable("EMPL");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.EmplNbr)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("EMPL_NBR");
            entity.Property(e => e.FrstNm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FRST_NM");
            entity.Property(e => e.LstNm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LST_NM");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.PlntOid).HasColumnName("PLNT_OID");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
            entity.Property(e => e.UsrId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USR_ID");

            entity.HasOne(d => d.PlntO).WithMany(p => p.Empls)
                .HasForeignKey(d => d.PlntOid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PLNT_EMPL_FK1");
        });

        modelBuilder.Entity<Gage>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("GAGE");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DimsActualOid).HasColumnName("DIMS_ACTUAL_OID");
            entity.Property(e => e.Gage1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GAGE");
            entity.Property(e => e.GageSn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GAGE_SN");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.DimsActualO).WithMany(p => p.Gages)
                .HasForeignKey(d => d.DimsActualOid)
                .HasConstraintName("FK_GAGE_DIMS_ACTUAL");
        });

        modelBuilder.Entity<MpmSrvr>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("MPM_SRVR_PK");

            entity.ToTable("MPM_SRVR");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdtBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPDT_BY");
            entity.Property(e => e.MpmDbNm)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MPM_DB_NM");
            entity.Property(e => e.MpmSrvrNm)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MPM_SRVR_NM");
            entity.Property(e => e.Uptms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPTMS");
        });

        modelBuilder.Entity<Mtrl>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("MTRL");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.MtrlNbr)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("MTRL_NBR");
            entity.Property(e => e.Updtms)
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
        });

        modelBuilder.Entity<PartSn>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("PART_SN");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DwgDimsOid).HasColumnName("DWG_DIMS_OID");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.PartSn1).HasColumnName("PART_SN");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.DwgDimsO).WithMany(p => p.PartSns)
                .HasForeignKey(d => d.DwgDimsOid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PART_SN_DWG_DIMS");
        });

        modelBuilder.Entity<Plnt>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PLNT_PK");

            entity.ToTable("PLNT");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg).HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.PlntCd)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("PLNT_CD");
            entity.Property(e => e.PlntDsc)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("PLNT_DSC");
            entity.Property(e => e.Updtms)
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
        });

        modelBuilder.Entity<PlntMpm>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PLNT_MPM_PK");

            entity.ToTable("PLNT_MPM");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdtBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPDT_BY");
            entity.Property(e => e.MpmSrvrOid).HasColumnName("MPM_SRVR_OID");
            entity.Property(e => e.PlntOid).HasColumnName("PLNT_OID");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.MpmSrvrO).WithMany(p => p.PlntMpms)
                .HasForeignKey(d => d.MpmSrvrOid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MPM_SRVR_PLNT_MPM_FK1");

            entity.HasOne(d => d.PlntO).WithMany(p => p.PlntMpms)
                .HasForeignKey(d => d.PlntOid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PLNT_PLNT_MPM_FK1");
        });

        modelBuilder.Entity<PrchOrdr>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("PRCH_ORDR");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.PlntOid).HasColumnName("PLNT_OID");
            entity.Property(e => e.PrchOrdrNbr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PRCH_ORDR_NBR");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.PlntO).WithMany(p => p.PrchOrdrs)
                .HasForeignKey(d => d.PlntOid)
                .HasConstraintName("FK_PRCH_ORDR_PLNT");
        });

        modelBuilder.Entity<PrchOrdrItm>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("PRCH_ORDR_ITM");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.MtrlOid).HasColumnName("MTRL_OID");
            entity.Property(e => e.PrchOrdrItmNbr)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("PRCH_ORDR_ITM_NBR");
            entity.Property(e => e.PrchOrdrOid).HasColumnName("PRCH_ORDR_OID");
            entity.Property(e => e.Qty)
                .HasDefaultValueSql("((1))")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("QTY");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.MtrlO).WithMany(p => p.PrchOrdrItms)
                .HasForeignKey(d => d.MtrlOid)
                .HasConstraintName("FK_PRCH_ORDR_ITM_MTRL");

            entity.HasOne(d => d.PrchOrdrO).WithMany(p => p.PrchOrdrItms)
                .HasForeignKey(d => d.PrchOrdrOid)
                .HasConstraintName("FK_PRCH_ORDR_ITM_PRCH_ORDR");
        });

        modelBuilder.Entity<ProdOrdr>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PROD_ORDR_PK");

            entity.ToTable("PROD_ORDR");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.BtchNbr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BTCH_NBR");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.MtrlOid).HasColumnName("MTRL_OID");
            entity.Property(e => e.PlntOid).HasColumnName("PLNT_OID");
            entity.Property(e => e.ProdOrdrNbr)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("PROD_ORDR_NBR");
            entity.Property(e => e.Qty)
                .HasDefaultValueSql("((1))")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("QTY");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");

            entity.HasOne(d => d.MtrlO).WithMany(p => p.ProdOrdrs)
                .HasForeignKey(d => d.MtrlOid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PROD_ORDR_MTRL");

            entity.HasOne(d => d.PlntO).WithMany(p => p.ProdOrdrs)
                .HasForeignKey(d => d.PlntOid)
                .HasConstraintName("PLNT_PROD_ORDR_FK1");
        });

        modelBuilder.Entity<Sqlbulkinsert>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SQLBULKINSERT");

            entity.Property(e => e.Col1).HasColumnName("COL1");
            entity.Property(e => e.Col2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COL2");
            entity.Property(e => e.Col3)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COL3");
        });

        modelBuilder.Entity<SrvrShr>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("SRVR_SHR_PK");

            entity.ToTable("SRVR_SHR");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.ShrNm)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SHR_NM");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
        });

        modelBuilder.Entity<TblAuditHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_AuditHistory");

            entity.Property(e => e.AuditLogId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AuditLogID");
            entity.Property(e => e.Balloon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChangedValue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CurrentValue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductionOrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblBaloonDrawingHeader>(entity =>
        {
            entity.HasKey(e => e.BaloonDrwID).HasName("PK_tbl_Baloon_Drawing");

            entity.ToTable("tbl_Baloon_Drawing_Header");

            entity.Property(e => e.BaloonDrwID).HasColumnName("BaloonDrwID");
            
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Part_Revision)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_Revision");
            entity.Property(e => e.ProductionOrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Revision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Total_Page_No).HasColumnName("Total_Page_No");
            entity.Property(e => e.RotateProperties).HasColumnName("RotateProperties");
        });

        modelBuilder.Entity<TblBaloonDrawingLiner>(entity =>
        {
            //   entity
            //      .HasNoKey()
            //     .ToTable("tbl_Baloon_Drawing_Liner");

           // entity.HasKey(e => e.DrawLineID).HasName("PK_tbl_Baloon_Drawing_Liner");

            entity.ToTable("tbl_Baloon_Drawing_Liner");

            entity.Property(e => e.DrawLineID).HasColumnName("DrawLineID");

            entity.HasIndex(e => new { e.BaloonDrwFileID, e.DrawingNumber, e.Revision }, "tbl_Baloon_Drawing_Liner_A1");
            entity.Property(e => e.Balloon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balloon_Text_FontSize).HasColumnName("Balloon_Text_FontSize");
            entity.Property(e => e.Balloon_Thickness).HasColumnName("Balloon_Thickness");
            entity.Property(e => e.BaloonDrwFileID)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BaloonDrwFileID");
            entity.Property(e => e.BaloonDrwID).HasColumnName("BaloonDrwID");
            entity.Property(e => e.Circle_Height).HasColumnName("Circle_Height");
            entity.Property(e => e.Circle_Width).HasColumnName("Circle_Width");
            entity.Property(e => e.Circle_X_Axis).HasColumnName("Circle_X_Axis");
            entity.Property(e => e.Circle_Y_Axis).HasColumnName("Circle_Y_Axis");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Crop_Height).HasColumnName("Crop_Height");
            entity.Property(e => e.Crop_Width).HasColumnName("Crop_Width");
            entity.Property(e => e.Crop_X_Axis).HasColumnName("Crop_X_Axis");
            entity.Property(e => e.Crop_Y_Axis).HasColumnName("Crop_Y_Axis");
            entity.Property(e => e.DrawLineID)
                .ValueGeneratedOnAdd()
                .HasColumnName("DrawLineID");
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaxTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Maximum)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MeasuredBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MeasuredOn).HasColumnType("datetime");
            entity.Property(e => e.MinTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Minimum)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MinusTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Nominal).IsUnicode(false);
            entity.Property(e => e.Page_No).HasColumnName("Page_No");
            entity.Property(e => e.Part_Revision)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_Revision");
            entity.Property(e => e.PlusTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductionOrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Revision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToleranceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ZoomFactor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsCritical).HasColumnName("IsCritical");
        });

        modelBuilder.Entity<TblComment>(entity =>
        {
            entity.ToTable("Tbl_Comments");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tbl_DBConfiguration");

            entity.ToTable("Tbl_Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Key)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Value).IsUnicode(false);
        });

        modelBuilder.Entity<TblConfigurationBkp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Configuration_bkp");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Key)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Value).IsUnicode(false);
        });

        modelBuilder.Entity<TblDbconfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tbl_DBConfiguration_1");

            entity.ToTable("Tbl_DBConfiguration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Authendication)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Datasource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Dbname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DBName");
            entity.Property(e => e.Environment)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<TblDimensionInputHeader>(entity =>
        {
            entity.HasKey(e => e.BaloonDrwId).HasName("PK_tbl_Dimension_Input");

            entity.ToTable("tbl_Dimension_Input_Header");

            entity.Property(e => e.BaloonDrwId).HasColumnName("BaloonDrwID");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.Batch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ConfirmationNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OperationNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Part)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartRevision)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_Revision");
            entity.Property(e => e.ProductionOrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Revision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalPageNo).HasColumnName("Total_Page_No");
        });

        modelBuilder.Entity<TblDimensionInputHistory>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("tbl_Dimension_Input_History_P");

            entity.ToTable("tbl_Dimension_Input_History");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Actual)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balloon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BaloonDrwFileId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BaloonDrwFileID");
            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Decision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DecisionBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GaugeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GaugeID");
            entity.Property(e => e.MeasuredBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MeasuredOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Operation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PageNo).HasColumnName("Page_No");
            entity.Property(e => e.ProductionOrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Revision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkCenter)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblDimensionInputLiner>(entity =>
        {
            entity.HasKey(e => e.DrawLineId);

            entity.ToTable("tbl_Dimension_Input_liner");

            entity.HasIndex(e => new { e.DrawingNumber, e.Revision, e.ProductionOrderNumber }, "tbl_Dimension_Input_Liner_A2");

            entity.HasIndex(e => new { e.DrawingNumber, e.Revision, e.ProductionOrderNumber, e.SerialNo }, "tbl_Dimension_Input_Liner_A3");

            entity.HasIndex(e => new { e.BaloonDrwFileId, e.DrawingNumber, e.Revision, e.ProductionOrderNumber, e.SerialNo }, "tbl_Dimension_Input_liner_A1");

            entity.Property(e => e.DrawLineId).HasColumnName("DrawLineID");
            entity.Property(e => e.Actual)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApproveStatus).HasColumnName("Approve_Status");
            entity.Property(e => e.Balloon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BalloonTextFontSize).HasColumnName("Balloon_Text_FontSize");
            entity.Property(e => e.BalloonThickness).HasColumnName("Balloon_Thickness");
            entity.Property(e => e.BaloonDrwFileId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BaloonDrwFileID");
            entity.Property(e => e.BaloonDrwId).HasColumnName("BaloonDrwID");
            entity.Property(e => e.CircleHeight).HasColumnName("Circle_Height");
            entity.Property(e => e.CircleWidth).HasColumnName("Circle_Width");
            entity.Property(e => e.CircleXAxis).HasColumnName("Circle_X_Axis");
            entity.Property(e => e.CircleYAxis).HasColumnName("Circle_Y_Axis");
            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CompletePercentage)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CropHeight).HasColumnName("Crop_Height");
            entity.Property(e => e.CropWidth).HasColumnName("Crop_Width");
            entity.Property(e => e.CropXAxis).HasColumnName("Crop_X_Axis");
            entity.Property(e => e.CropYAxis).HasColumnName("Crop_Y_Axis");
            entity.Property(e => e.Decision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DecisionBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DimensionChecked).HasColumnName("Dimension_Checked");
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FolderDwngreport)
                .IsUnicode(false)
                .HasColumnName("Folder_DWNGReport");
            entity.Property(e => e.FolderExcelreport)
                .IsUnicode(false)
                .HasColumnName("Folder_EXCELReport");
            entity.Property(e => e.FolderPdfreport)
                .IsUnicode(false)
                .HasColumnName("Folder_PDFReport");
            entity.Property(e => e.GaugeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GaugeID");
            entity.Property(e => e.MaxTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Maximum)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MeasuredBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MeasuredOn).HasColumnType("datetime");
            entity.Property(e => e.MinTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Minimum)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MinusTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Nominal).IsUnicode(false);
            entity.Property(e => e.Operation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PageNo).HasColumnName("Page_No");
            entity.Property(e => e.PlusTolerance)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductionOrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RemarksonlyforQcInput)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Revision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToleranceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkCenter)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ZoomFactor).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblMeasureSubType>(entity =>
        {
            entity.HasKey(e => e.SubTypeId);

            entity.ToTable("Tbl_Measure_SubType");

            entity.Property(e => e.SubTypeId).HasColumnName("SubType_ID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SubTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SubType_Name");
            entity.Property(e => e.TypeId).HasColumnName("Type_ID");
        });

        modelBuilder.Entity<TblMeasureType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("Tbl_MeasureType");

            entity.Property(e => e.TypeId).HasColumnName("Type_ID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Type_Name");
        });

        modelBuilder.Entity<TblOriginalBalloonedImageDetail>(entity =>
        {
            entity.HasKey(e => e.FileHeaderId);

            entity.ToTable("Tbl_Original_Ballooned_Image_Details");

            entity.Property(e => e.FileHeaderId).HasColumnName("File_Header_ID");
            entity.Property(e => e.BalloonHeaderId).HasColumnName("Balloon_Header_ID");
            entity.Property(e => e.BaloonDrwFileId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BaloonDrwFileID");
            entity.Property(e => e.BaloonFileName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BaloonFilePath).IsUnicode(false);
            entity.Property(e => e.BaloonFileType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DrawingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImageHeight).HasColumnName("Image_Height");
            entity.Property(e => e.ImageWidth).HasColumnName("Image_Width");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OriginalFile).HasColumnName("Original_File");
            entity.Property(e => e.OriginalPath)
                .IsUnicode(false)
                .HasColumnName("Original_Path");
            entity.Property(e => e.PageNo).HasColumnName("Page_No");
            entity.Property(e => e.Revision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPageNo).HasColumnName("Total_Page_No");
        });

        modelBuilder.Entity<TblUom>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK_X_UOM");

            entity.ToTable("Tbl_UOM");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.Dsc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DSC");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.UomCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_Users");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblUserAccess>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_UserAccess");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WorkCenter)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblWorkCenter>(entity =>
        {
            entity.ToTable("Tbl_WorkCenter");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.WorkCenter)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Work_Center");
        });

        modelBuilder.Entity<XDimType>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("DIM_TYPE_PK");

            entity.ToTable("X_DIM_TYPE");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.DimType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DIM_TYPE");
            entity.Property(e => e.LstUpdtBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPDT_BY");
            entity.Property(e => e.Updtms)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
        });

        modelBuilder.Entity<XSrvrType>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("X_SRVR_TYPE_PK");

            entity.ToTable("X_SRVR_TYPE");

            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.ActvFlg).HasColumnName("ACTV_FLG");
            entity.Property(e => e.Crtms)
                .HasColumnType("datetime")
                .HasColumnName("CRTMS");
            entity.Property(e => e.LstUpdBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LST_UPD_BY");
            entity.Property(e => e.SrvrTypeCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SRVR_TYPE_CD");
            entity.Property(e => e.TypeNm)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TYPE_NM");
            entity.Property(e => e.Updtms)
                .HasColumnType("datetime")
                .HasColumnName("UPDTMS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
