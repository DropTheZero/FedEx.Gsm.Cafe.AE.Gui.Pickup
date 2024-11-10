// Decompiled with JetBrains decompiler
// Type: FedEx.Gsm.Cafe.ApplicationEngine.Gui.PickupModule.frmPickupConfirmationDlg
// Assembly: FedEx.Gsm.Cafe.AE.Gui.Pickup, Version=38.55.1083.0, Culture=neutral, PublicKeyToken=null
// MVID: CD4D2EC4-544E-4A91-B94A-E238E0D49609
// Assembly location: C:\Program Files (x86)\FedEx\ShipManager\BIN\FedEx.Gsm.Cafe.AE.Gui.Pickup.dll

using FedEx.Gsm.Cafe.ApplicationEngine.Gui.UtilityFunctions;
using FedEx.Gsm.ShipEngine.Entities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace FedEx.Gsm.Cafe.ApplicationEngine.Gui.PickupModule
{
  public class frmPickupConfirmationDlg : HelpFormBase
  {
    private PickupInfo _pi;
    private Font printFont;
    private IContainer components;
    private Label lblPickupStatusTitle;
    private Label lblReservationNbr;
    private Label lblStatus;
    private Label lblReservationDate;
    private Label lblTotalPackages;
    private Label lblTotalWeight;
    private Label lblCompany;
    private Label lblAddress;
    private Label lblCity;
    private Label lblPostal;
    private Label lblContact;
    private Label lblCountry;
    private Label lblComments;
    private Label lblReadyTime;
    private Label lblClosingTime;
    private Label txtReservationNbr;
    private Label txtStatus;
    private Label txtReservationDate;
    private Label txtTotalPackages;
    private Label txtTotalWeight;
    private Label txtCompany;
    private Label txtAddress;
    private Label txtCity;
    private Label txtPostal;
    private Label txtContact;
    private Label txtCountry;
    private Label txtComments;
    private Label txtReadyTime;
    private Label txtClosingTime;
    private Button btnPrint;
    private Button btnOk;
    private Panel panelStatus;
    private Label lblErrorMsg;
    private Label lblPickupType;
    private Label txtPickupType;

    public frmPickupConfirmationDlg(PickupInfo pi)
    {
      this.InitializeComponent();
      this._pi = pi;
      this.txtReservationNbr.Text = pi.DispatchLocId + pi.ReservationNbr;
      this.txtStatus.Text = pi.StatusCode.ToString();
      this.txtReservationDate.Text = pi.PickupDate.ToString("dd-MMM-yyyy");
      this.txtTotalPackages.Text = pi.TotalPackages.ToString();
      this.txtTotalWeight.Text = pi.TotalWeight.ToString() + " " + pi.WeightType;
      this.txtCompany.Text = pi.CompanyName;
      this.txtContact.Text = pi.ContactName;
      this.txtAddress.Text = pi.Address1;
      this.txtCity.Text = pi.City;
      this.txtPostal.Text = pi.Postal;
      this.txtCountry.Text = pi.CountryCode;
      this.txtComments.Text = pi.Remarks;
      this.txtReadyTime.Text = pi.ReadyTime.ToString("HH:mm");
      this.txtClosingTime.Text = pi.ClosingTime.ToString("HH:mm");
      if (pi.pickupIndicator == "D")
        this.txtPickupType.Text = "Domestic";
      else
        this.txtPickupType.Text = "International";
      if (string.IsNullOrEmpty(pi.ErrorMessage))
        this.lblErrorMsg.Text = string.Empty;
      else
        this.lblErrorMsg.Text = PickupView.GetErrorMessageFromErrorCode(pi.ErrorMessage);
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (this.printFont == null)
        this.printFont = new Font("Arial", 12f);
      using (PrintDocument printDocument = new PrintDocument())
      {
        printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
        printDocument.Print();
      }
    }

    private void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
      float left = (float) ev.MarginBounds.Left;
      float top = (float) ev.MarginBounds.Top;
      string[] reportStrings = this.GetReportStrings();
      double num = (double) ev.MarginBounds.Height / (double) this.printFont.GetHeight(ev.Graphics);
      for (int index = 0; index < reportStrings.Length; ++index)
      {
        string s = reportStrings[index];
        float y = top + (float) index * this.printFont.GetHeight(ev.Graphics);
        ev.Graphics.DrawString(s, this.printFont, Brushes.Black, left, y, new StringFormat());
      }
      ev.HasMorePages = false;
    }

    private void btnOk_Click(object sender, EventArgs e) => this.Close();

    private string[] GetReportStrings()
    {
      return ((string) null + this.lblPickupType.Text + ": " + this._pi.PickupType.ToString() + "~" + this.lblReservationNbr.Text + ": " + this._pi.DispatchLocId + this._pi.ReservationNbr + "~" + this.lblStatus.Text + ": " + this._pi.StatusCode.ToString() + "~" + this._pi.ErrorMessage + "~" + this.lblReservationDate.Text + ": " + this._pi.PickupDate.ToString("dd-MMM-yyyy") + "~" + this.lblTotalPackages.Text + ": " + this._pi.TotalPackages.ToString() + "~" + this.lblTotalWeight.Text + ": " + this._pi.TotalWeight.ToString() + " " + this._pi.WeightType + "~" + this.lblCompany.Text + ": " + this._pi.CompanyName + "~" + this.lblContact.Text + ": " + this._pi.ContactName + "~" + this.lblAddress.Text + ": " + this._pi.Address1 + "~" + this.lblCity.Text + ": " + this._pi.City + "~" + this.lblPostal.Text + ": " + this._pi.Postal + "~" + this.lblCountry.Text + ": " + this._pi.CountryCode + "~" + " ~" + " ~" + " ~" + " ~" + this.lblComments.Text + ": " + this._pi.Remarks + "~" + this.lblReadyTime.Text + ": " + this._pi.ReadyTime.ToString("HH:mm") + "~" + this.lblClosingTime.Text + ": " + this._pi.ClosingTime.ToString("HH:mm")).Split('~');
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmPickupConfirmationDlg));
      this.lblPickupStatusTitle = new Label();
      this.lblReservationNbr = new Label();
      this.lblStatus = new Label();
      this.lblReservationDate = new Label();
      this.lblTotalPackages = new Label();
      this.lblTotalWeight = new Label();
      this.lblCompany = new Label();
      this.lblAddress = new Label();
      this.lblCity = new Label();
      this.lblPostal = new Label();
      this.lblContact = new Label();
      this.lblCountry = new Label();
      this.lblComments = new Label();
      this.lblReadyTime = new Label();
      this.lblClosingTime = new Label();
      this.txtReservationNbr = new Label();
      this.txtStatus = new Label();
      this.txtReservationDate = new Label();
      this.txtTotalPackages = new Label();
      this.txtTotalWeight = new Label();
      this.txtCompany = new Label();
      this.txtAddress = new Label();
      this.txtCity = new Label();
      this.txtPostal = new Label();
      this.txtContact = new Label();
      this.txtCountry = new Label();
      this.txtComments = new Label();
      this.txtReadyTime = new Label();
      this.txtClosingTime = new Label();
      this.btnPrint = new Button();
      this.btnOk = new Button();
      this.panelStatus = new Panel();
      this.lblErrorMsg = new Label();
      this.lblPickupType = new Label();
      this.txtPickupType = new Label();
      this.panelStatus.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblPickupStatusTitle, "lblPickupStatusTitle");
      this.lblPickupStatusTitle.Name = "lblPickupStatusTitle";
      componentResourceManager.ApplyResources((object) this.lblReservationNbr, "lblReservationNbr");
      this.lblReservationNbr.Name = "lblReservationNbr";
      componentResourceManager.ApplyResources((object) this.lblStatus, "lblStatus");
      this.lblStatus.Name = "lblStatus";
      componentResourceManager.ApplyResources((object) this.lblReservationDate, "lblReservationDate");
      this.lblReservationDate.Name = "lblReservationDate";
      componentResourceManager.ApplyResources((object) this.lblTotalPackages, "lblTotalPackages");
      this.lblTotalPackages.Name = "lblTotalPackages";
      componentResourceManager.ApplyResources((object) this.lblTotalWeight, "lblTotalWeight");
      this.lblTotalWeight.Name = "lblTotalWeight";
      componentResourceManager.ApplyResources((object) this.lblCompany, "lblCompany");
      this.lblCompany.Name = "lblCompany";
      componentResourceManager.ApplyResources((object) this.lblAddress, "lblAddress");
      this.lblAddress.Name = "lblAddress";
      componentResourceManager.ApplyResources((object) this.lblCity, "lblCity");
      this.lblCity.Name = "lblCity";
      componentResourceManager.ApplyResources((object) this.lblPostal, "lblPostal");
      this.lblPostal.Name = "lblPostal";
      componentResourceManager.ApplyResources((object) this.lblContact, "lblContact");
      this.lblContact.Name = "lblContact";
      componentResourceManager.ApplyResources((object) this.lblCountry, "lblCountry");
      this.lblCountry.Name = "lblCountry";
      componentResourceManager.ApplyResources((object) this.lblComments, "lblComments");
      this.lblComments.Name = "lblComments";
      componentResourceManager.ApplyResources((object) this.lblReadyTime, "lblReadyTime");
      this.lblReadyTime.Name = "lblReadyTime";
      componentResourceManager.ApplyResources((object) this.lblClosingTime, "lblClosingTime");
      this.lblClosingTime.Name = "lblClosingTime";
      componentResourceManager.ApplyResources((object) this.txtReservationNbr, "txtReservationNbr");
      this.txtReservationNbr.Name = "txtReservationNbr";
      componentResourceManager.ApplyResources((object) this.txtStatus, "txtStatus");
      this.txtStatus.Name = "txtStatus";
      componentResourceManager.ApplyResources((object) this.txtReservationDate, "txtReservationDate");
      this.txtReservationDate.Name = "txtReservationDate";
      componentResourceManager.ApplyResources((object) this.txtTotalPackages, "txtTotalPackages");
      this.txtTotalPackages.Name = "txtTotalPackages";
      componentResourceManager.ApplyResources((object) this.txtTotalWeight, "txtTotalWeight");
      this.txtTotalWeight.Name = "txtTotalWeight";
      componentResourceManager.ApplyResources((object) this.txtCompany, "txtCompany");
      this.txtCompany.Name = "txtCompany";
      componentResourceManager.ApplyResources((object) this.txtAddress, "txtAddress");
      this.txtAddress.Name = "txtAddress";
      componentResourceManager.ApplyResources((object) this.txtCity, "txtCity");
      this.txtCity.Name = "txtCity";
      componentResourceManager.ApplyResources((object) this.txtPostal, "txtPostal");
      this.txtPostal.Name = "txtPostal";
      componentResourceManager.ApplyResources((object) this.txtContact, "txtContact");
      this.txtContact.Name = "txtContact";
      componentResourceManager.ApplyResources((object) this.txtCountry, "txtCountry");
      this.txtCountry.Name = "txtCountry";
      componentResourceManager.ApplyResources((object) this.txtComments, "txtComments");
      this.txtComments.Name = "txtComments";
      componentResourceManager.ApplyResources((object) this.txtReadyTime, "txtReadyTime");
      this.txtReadyTime.Name = "txtReadyTime";
      componentResourceManager.ApplyResources((object) this.txtClosingTime, "txtClosingTime");
      this.txtClosingTime.Name = "txtClosingTime";
      componentResourceManager.ApplyResources((object) this.btnPrint, "btnPrint");
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnOk.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
      this.btnOk.Name = "btnOk";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.panelStatus.Controls.Add((Control) this.lblPickupType);
      this.panelStatus.Controls.Add((Control) this.txtPickupType);
      this.panelStatus.Controls.Add((Control) this.lblErrorMsg);
      this.panelStatus.Controls.Add((Control) this.lblCompany);
      this.panelStatus.Controls.Add((Control) this.lblReservationNbr);
      this.panelStatus.Controls.Add((Control) this.lblStatus);
      this.panelStatus.Controls.Add((Control) this.txtClosingTime);
      this.panelStatus.Controls.Add((Control) this.lblReservationDate);
      this.panelStatus.Controls.Add((Control) this.txtReadyTime);
      this.panelStatus.Controls.Add((Control) this.lblTotalPackages);
      this.panelStatus.Controls.Add((Control) this.txtComments);
      this.panelStatus.Controls.Add((Control) this.lblTotalWeight);
      this.panelStatus.Controls.Add((Control) this.txtCountry);
      this.panelStatus.Controls.Add((Control) this.lblAddress);
      this.panelStatus.Controls.Add((Control) this.txtContact);
      this.panelStatus.Controls.Add((Control) this.lblCity);
      this.panelStatus.Controls.Add((Control) this.txtPostal);
      this.panelStatus.Controls.Add((Control) this.lblPostal);
      this.panelStatus.Controls.Add((Control) this.txtCity);
      this.panelStatus.Controls.Add((Control) this.lblContact);
      this.panelStatus.Controls.Add((Control) this.txtAddress);
      this.panelStatus.Controls.Add((Control) this.lblCountry);
      this.panelStatus.Controls.Add((Control) this.txtCompany);
      this.panelStatus.Controls.Add((Control) this.lblComments);
      this.panelStatus.Controls.Add((Control) this.txtTotalWeight);
      this.panelStatus.Controls.Add((Control) this.lblReadyTime);
      this.panelStatus.Controls.Add((Control) this.txtTotalPackages);
      this.panelStatus.Controls.Add((Control) this.lblClosingTime);
      this.panelStatus.Controls.Add((Control) this.txtReservationDate);
      this.panelStatus.Controls.Add((Control) this.txtReservationNbr);
      this.panelStatus.Controls.Add((Control) this.txtStatus);
      componentResourceManager.ApplyResources((object) this.panelStatus, "panelStatus");
      this.panelStatus.Name = "panelStatus";
      this.lblErrorMsg.BackColor = Color.Transparent;
      this.lblErrorMsg.ForeColor = Color.Red;
      componentResourceManager.ApplyResources((object) this.lblErrorMsg, "lblErrorMsg");
      this.lblErrorMsg.Name = "lblErrorMsg";
      componentResourceManager.ApplyResources((object) this.lblPickupType, "lblPickupType");
      this.lblPickupType.Name = "lblPickupType";
      this.helpProvider1.SetShowHelp((Control) this.lblPickupType, (bool) componentResourceManager.GetObject("lblPickupType.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.txtPickupType, "txtPickupType");
      this.txtPickupType.Name = "txtPickupType";
      this.helpProvider1.SetShowHelp((Control) this.txtPickupType, (bool) componentResourceManager.GetObject("txtPickupType.ShowHelp"));
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelStatus);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.lblPickupStatusTitle);
      this.helpProvider1.SetHelpKeyword((Control) this, componentResourceManager.GetString("$this.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this, (HelpNavigator) componentResourceManager.GetObject("$this.HelpNavigator"));
      this.Name = nameof (frmPickupConfirmationDlg);
      this.helpProvider1.SetShowHelp((Control) this, (bool) componentResourceManager.GetObject("$this.ShowHelp"));
      this.panelStatus.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
