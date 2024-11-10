// Decompiled with JetBrains decompiler
// Type: FedEx.Gsm.Cafe.ApplicationEngine.Gui.PickupModule.PickupView
// Assembly: FedEx.Gsm.Cafe.AE.Gui.Pickup, Version=38.55.1083.0, Culture=neutral, PublicKeyToken=null
// MVID: CD4D2EC4-544E-4A91-B94A-E238E0D49609
// Assembly location: C:\Program Files (x86)\FedEx\ShipManager\BIN\FedEx.Gsm.Cafe.AE.Gui.Pickup.dll

using FedEx.Gsm.Cafe.ApplicationEngine.Gui.Data;
using FedEx.Gsm.Cafe.ApplicationEngine.Gui.Eventing;
using FedEx.Gsm.Cafe.ApplicationEngine.Gui.UserControls;
using FedEx.Gsm.Cafe.ApplicationEngine.Gui.UtilityFunctions;
using FedEx.Gsm.Common.Logging;
using FedEx.Gsm.ShipEngine.DataAccess;
using FedEx.Gsm.ShipEngine.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace FedEx.Gsm.Cafe.ApplicationEngine.Gui.PickupModule
{
  public class PickupView : UserControlHelpEx, IBeforeShow
  {
    private ToolStripButton _tabItem = new ToolStripButton();
    private string _meter;
    private string _account;
    private PickupInfo _pickupInfo;
    private string _messageTextOriginalText;
    private ComponentResourceManager resources;
    private IContainer components;
    private Panel panelPickupHistory;
    private Panel panelPickupInfo;
    private TextBox edtAddress2;
    private TextBox edtAddress1;
    private TextBox edtCompanyName;
    private TextBox edtContactName;
    private ComboBox cboCountry;
    private ComboBox cboSenderId;
    private Label lblAddress2;
    private Label lblAddress1;
    private Label lblCompanyName;
    private Label lblContact;
    private Label lblCountry;
    private Label lblSenderId;
    private Label lblTelephoneExtension;
    private Label lblState;
    private Label lblTelephone;
    private Label lblCity;
    private Label lblPostal;
    private FdxMaskedEdit edtTelephoneExtension;
    private ComboBox edtCity;
    private ComboBox cboStateProvince;
    private Panel panelPackageDetails;
    private Label lblPickupDate;
    private ComboBox cboWeightType;
    private FdxMaskedEdit edtTotalWeight;
    private FdxMaskedEdit edtTotalPkgs;
    private Label lblTotalWeight;
    private Label lblTtlPkgs;
    private Label lblDescription;
    private Label lblClosingTime;
    private Label lblReadyTime;
    private CheckBoxEx chkResidential;
    private FdxMaskedEdit edtPostal;
    private FdxMaskedEdit edtTelephone;
    private HeaderText htPickupInfo;
    private HeaderText htPackageDetails;
    private HeaderText htPickupHistory;
    private PictureBox verticalLine;
    private FdxMaskedEdit edtClosingTime;
    private FdxMaskedEdit edtReadyTime;
    private TextBox edtDescription;
    private Panel panelBottomButtons;
    private Button btnSubmitReservation;
    private Button btnPrintReservation;
    private Button btnCancelPickup;
    private Button btnSchedulePickup;
    private Button btnDeleteReservation;
    private Button btnClearFields;
    private ImageList imageList1;
    private Button btnCalendar;
    private Label lblMessageRight;
    private Label lblMessageTop;
    private Label lblMessageBottom;
    private Label lblMessageLeft;
    private Label lblMessageText;
    private Panel panelMessage;
    private FdxMaskedEdit edtPickupDate;
    private DataGridView dgPickupHistory;
    private BackgroundWorker bwDispatchThread;
    private BackgroundWorker bwCancelDispatchThread;
    private DataGridViewTextBoxColumn ReservationNbr;
    private DataGridViewTextBoxColumn PickupDate;
    private DataGridViewTextBoxColumn PickupTime;
    private DataGridViewTextBoxColumn ContactName;
    private DataGridViewTextBoxColumn Status;
    private DataGridViewTextBoxColumn PickupId;
    private DataGridViewTextBoxColumn PickupType;
    private Label lblPickupType;
    private ComboBox cboPickupType;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

    public ToolStripItem TabItem => (ToolStripItem) this._tabItem;

    public PickupView()
    {
      string name = (string) null;
      new FedEx.Gsm.Common.ConfigManager.ConfigManager(FedEx.Gsm.Common.ConfigManager.ConfigManager.Sections.GUI).GetProfileString("Settings", "Language", out name);
      try
      {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(name, false);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(name, false);
      }
      catch (Exception ex)
      {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
      }
      this.InitializeComponent();
      this.TabStop = true;
      if (this.DesignMode)
        return;
      this._tabItem.Text = GuiData.Languafier.TranslateError(20670);
      this._tabItem.Padding = new Padding(5, 0, 5, 0);
      this._tabItem.Name = "toolStripButtonPickup";
      this._tabItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this._pickupInfo = new PickupInfo();
      this.resources = new ComponentResourceManager(typeof (PickupView));
      this.SetupEvents();
      this._messageTextOriginalText = this.lblMessageText.Text;
      FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Verbose, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, "Meter: " + this._meter + " Acct: " + this._account);
    }

    public void BeforeShow(object sender, BeforeShowEventArgs args)
    {
    }

    private void SetupEvents()
    {
      if (GuiData.EventBroker == null)
        return;
      GuiData.EventBroker.AddSubscriber(EventBroker.Events.CurrentAccountChanged, (object) this, "OnCurrentAccountChanged");
      GuiData.EventBroker.AddSubscriber(EventBroker.Events.SenderListChanged, (object) this, "OnSenderListChanged");
    }

    private void RemoveEvents()
    {
      try
      {
        GuiData.EventBroker.RemoveSubscriber(EventBroker.Events.CurrentAccountChanged, (object) this, "OnCurrentAccountChanged");
        GuiData.EventBroker.RemoveSubscriber(EventBroker.Events.SenderListChanged, (object) this, "OnSenderListChanged");
      }
      catch
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      if (disposing)
        this.RemoveEvents();
      base.Dispose(disposing);
    }

    public void OnCurrentAccountChanged(object sender, AccountEventArgs args)
    {
      if (args.OldAccount == null || args.OldAccount.DateFormat != args.NewAccount.DateFormat)
        Utility.SetDateMasks(this.edtPickupDate);
      this.edtPickupDate.Text = Utility.DateToText(DateTime.Today);
      this.PopulatePickupHistory();
    }

    public void OnSenderListChanged(object sender, EventArgs args) => this.PopulateComboLists();

    public bool InitAccountSettings()
    {
      Account filter1 = new Account();
      Account output1 = (Account) null;
      Error error1 = (Error) null;
      FedEx.Gsm.Common.ConfigManager.ConfigManager configManager = new FedEx.Gsm.Common.ConfigManager.ConfigManager(FedEx.Gsm.Common.ConfigManager.ConfigManager.Sections.GUI);
      string str1;
      configManager.GetProfileString("SETTINGS", "METER", out str1);
      string str2;
      configManager.GetProfileString("SETTINGS", "ACCOUNT", out str2);
      if (str1 != null)
        this._meter = str1;
      if (str2 != null)
        this._account = str2;
      filter1.AccountNumber = this._account;
      filter1.MeterNumber = this._meter;
      if (GuiData.AppController.ShipEngine.Retrieve<Account>(filter1, out output1, out error1) == 1)
      {
        if (GuiData.CurrentAccount == null || GuiData.CurrentAccount.AccountNumber != output1.AccountNumber || GuiData.CurrentAccount.MeterNumber != output1.MeterNumber)
        {
          SystemPrefs filter2 = new SystemPrefs();
          filter2.FedExAcctNbr = output1.AccountNumber;
          filter2.MeterNumber = output1.MeterNumber;
          SystemPrefs output2 = (SystemPrefs) null;
          Error error2 = (Error) null;
          if (GuiData.AppController.ShipEngine.Retrieve<SystemPrefs>(filter2, out output2, out error2) == 1)
            GuiData.CurrentSystemPrefs = output2;
        }
        GuiData.CurrentAccount = output1;
      }
      FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Verbose, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, "Meter: " + this._meter + " Acct: " + this._account);
      return true;
    }

    private void btnClearFields_Click(object sender, EventArgs e)
    {
      this.ResetDefaultView();
      this.PopulateComboLists();
    }

    private void btnSchedulePickup_Click(object sender, EventArgs e)
    {
      this.SuspendLayout();
      this.panelPickupInfo.Enabled = true;
      this.panelPackageDetails.Enabled = true;
      this.btnClearFields.Enabled = true;
      this.btnSubmitReservation.Enabled = true;
      this.btnCancelPickup.Enabled = false;
      this.btnDeleteReservation.Enabled = false;
      this.btnSchedulePickup.Enabled = false;
      this.btnPrintReservation.Enabled = false;
      this.panelMessage.Visible = true;
      this.lblMessageText.Text = this._messageTextOriginalText;
      this.ClearAll();
      this.PopulateComboLists();
      if (this.dgPickupHistory.RowCount > 0)
        this.dgPickupHistory.CurrentRow.Selected = false;
      this.ResumeLayout();
    }

    private void PickupView_Load(object sender, EventArgs e)
    {
      if (this.DesignMode)
        return;
      Utility.HouseKeeping(this.Controls);
      this.InitAccountSettings();
      this.PopulatePickupHistory();
      this.ResetDefaultView();
      this.PopulateComboLists();
    }

    private void ResetDefaultView()
    {
      this.ClearAll();
      this.panelPickupHistory.Enabled = true;
      this.panelPackageDetails.Enabled = false;
      this.panelPickupInfo.Enabled = false;
      this.panelMessage.Visible = false;
      this.btnClearFields.Enabled = false;
      this.btnSubmitReservation.Enabled = false;
      this.btnCancelPickup.Enabled = false;
      this.btnDeleteReservation.Enabled = false;
      this.btnSchedulePickup.Enabled = true;
      this.btnPrintReservation.Enabled = false;
      this.cboWeightType.SelectedIndex = 1;
      this.btnSchedulePickup.Focus();
      if (this.dgPickupHistory.RowCount > 0)
        this.dgPickupHistory.CurrentRow.Selected = false;
      this.cboPickupType.Visible = false;
      this.lblPickupType.Visible = false;
    }

    private string GetStatusStringFromCode(PickupInfo.Status status)
    {
      string statusStringFromCode;
      switch (status)
      {
        case PickupInfo.Status.Scheduled:
          statusStringFromCode = "Scheduled";
          break;
        case PickupInfo.Status.Cancelled:
          statusStringFromCode = "Cancelled";
          break;
        case PickupInfo.Status.Failed:
          statusStringFromCode = "Failed";
          break;
        default:
          statusStringFromCode = "";
          break;
      }
      return statusStringFromCode;
    }

    private void PopulatePickupHistory()
    {
      string s = "0";
      string sortType = "0";
      FedEx.Gsm.Common.ConfigManager.ConfigManager configManager = new FedEx.Gsm.Common.ConfigManager.ConfigManager(FedEx.Gsm.Common.ConfigManager.ConfigManager.Sections.GUI);
      configManager.GetProfileString("SETTINGS/PICKUP", "SortByColumn", out s);
      configManager.GetProfileString("SETTINGS/PICKUP", "SortOrder", out sortType);
      try
      {
        this.dgPickupHistory.DataSource = (object) null;
        this.dgPickupHistory.Rows.Clear();
        PickupInfoList pickupInfoList = new PickupInfoList();
        PickupInfoList pickupHistory = GuiData.AppController.ShipEngine.GetPickupHistory(this._meter, this._account, new Error());
        if (pickupHistory != null)
        {
          foreach (PickupInfo pickupInfo in (List<PickupInfo>) pickupHistory)
            this.dgPickupHistory.Rows[this.dgPickupHistory.Rows.Add((object[]) new string[7]
            {
              pickupInfo.DispatchLocId + pickupInfo.ReservationNbr,
              Utility.DateToText(pickupInfo.PickupDate),
              pickupInfo.ReadyTime.ToString("HH:mm"),
              pickupInfo.ContactName,
              this.GetStatusStringFromCode(pickupInfo.StatusCode),
              pickupInfo.PickupId.ToString(),
              pickupInfo.pickupIndicator
            })].Cells["Status"].Tag = (object) pickupInfo.StatusCode;
        }
        if (pickupHistory.Count > 0)
        {
          int result;
          int.TryParse(s, out result);
          DataGridViewColumn column = this.dgPickupHistory.Columns[result];
          ListSortDirection gridSortTypeEnum = this.GetDataGridSortTypeEnum(sortType);
          this.dgPickupHistory.Sort(column, gridSortTypeEnum);
          column.HeaderCell.SortGlyphDirection = gridSortTypeEnum == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
        }
        else
          this.ResetDefaultView();
      }
      catch (Exception ex)
      {
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, ex.Message);
      }
    }

    private ListSortDirection GetDataGridSortTypeEnum(string sortType)
    {
      return !sortType.Equals("Ascending") ? ListSortDirection.Descending : ListSortDirection.Ascending;
    }

    private void PopulateComboLists()
    {
      DataTable output;
      GuiData.AppController.ShipEngine.GetDataList((object) null, GsmDataAccess.ListSpecification.Sender_List, out output, (List<GsmFilter>) null, (List<GsmSort>) null, (List<string>) null, new Error());
      if (output != null)
      {
        output.Columns.Add(new DataColumn("CodeAndName", typeof (string), "IIF(SenderCode <> '',SenderCode + ' - ' + SenderName, 'Current sender')"));
        this.cboSenderId.DisplayMember = "CodeAndName";
        this.cboSenderId.ValueMember = "SenderCode";
        this.cboSenderId.DataSource = (object) output;
      }
      else
        this.cboSenderId.DataSource = (object) output;
      try
      {
        Utility.PopulateCountryCombo(this.cboCountry, Utility.CountryComboType.Origin);
      }
      catch (Exception ex)
      {
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, "Error getting countries" + ex.Message);
      }
      Utility.SetDisplayAndValue(this.cboPickupType, Utility.GetDataTable(Utility.ListTypes.PickupType), "Description", "Code", false);
      this.cboPickupType.Text = "International";
    }

    private void PostProcessSenderCode(string code)
    {
      Sender filter = new Sender();
      Sender output = (Sender) null;
      Error error = (Error) null;
      filter.Code = code;
      if (GuiData.AppController.ShipEngine.Retrieve<Sender>(filter, out output, out error) == 1)
        this.SenderToScreen(output);
      else
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, "Couldn't retrieve Senderlist");
    }

    private void SenderToScreen(Sender sender)
    {
      this.cboSenderId.SelectedValue = (object) sender.Code;
      this.cboCountry.SelectedValue = (object) sender.Address.CountryCode;
      this.edtContactName.Text = sender.Address.Contact;
      this.edtCompanyName.Text = sender.Address.Company;
      this.edtAddress1.Text = sender.Address.Addr1;
      this.edtAddress2.Text = sender.Address.Addr2;
      this.edtCity.Text = sender.Address.City;
      this.cboStateProvince.SelectedValue = (object) sender.Address.StateProvince;
      this.edtPostal.Text = sender.Address.PostalCode;
      this.edtTelephone.Text = sender.Address.Phone;
      this.edtTelephoneExtension.Text = sender.Address.PhoneExtension;
      this.chkResidential.Checked = sender.IsResidential;
    }

    private void SetMasks(string country)
    {
      switch (country)
      {
        case "US":
          this.edtPostal.SetMask("99999-####");
          this.edtTelephone.SetMask(eMasks.maskPhoneTen);
          break;
        case "CA":
          this.edtPostal.SetMask(eMasks.maskCanadianZipCode);
          this.edtTelephone.SetMask(eMasks.maskPhoneTen);
          break;
        default:
          this.edtPostal.SetMask(eMasks.maskIntlZipCode);
          this.edtTelephone.SetMask(eMasks.maskIntlPhone);
          break;
      }
    }

    private void btnCalendar_Click(object sender, EventArgs e)
    {
      DatePicker datePicker = new DatePicker();
      datePicker.DateSelected = this.edtPickupDate.Text;
      if (datePicker.ShowDialog() != DialogResult.OK)
        return;
      Utility.SetDateMasks(this.edtPickupDate);
      this.edtPickupDate.Text = Utility.DateToText(datePicker.DateTimeSelected);
    }

    private void PickupInfoObjectToScreen(PickupInfo pi)
    {
      this.cboSenderId.SelectedValue = (object) pi.SenderCode;
      this.cboCountry.SelectedValue = (object) pi.CountryCode;
      this.edtContactName.Text = pi.ContactName;
      this.edtCompanyName.Text = pi.CompanyName;
      this.edtAddress1.Text = pi.Address1;
      this.edtAddress2.Text = pi.Address2;
      this.edtCity.Text = pi.City;
      this.cboStateProvince.SelectedValue = (object) pi.StateProvCode;
      this.edtPostal.Text = pi.Postal;
      this.edtTelephone.Text = pi.Phone;
      this.edtTelephoneExtension.Text = pi.PhoneExt;
      this.edtTotalPkgs.Text = pi.TotalPackages.ToString();
      this.edtTotalWeight.Text = pi.TotalWeight.ToString();
      this.cboWeightType.SelectedValue = (object) pi.WeightType;
      this.edtPickupDate.Text = Utility.DateToText(pi.PickupDate);
      this.edtReadyTime.Text = pi.ReadyTime.ToString("HH:mm");
      this.edtClosingTime.Text = pi.ClosingTime.ToString("HH:mm");
      this.edtDescription.Text = pi.Remarks;
      this.chkResidential.Checked = pi.LocationType == "R";
      if (!string.IsNullOrEmpty(pi.ErrorMessage))
      {
        this.panelMessage.Visible = true;
        this.lblMessageText.Text = PickupView.GetErrorMessageFromErrorCode(pi.ErrorMessage);
      }
      else
      {
        this.panelMessage.Visible = false;
        this.lblMessageText.Text = string.Empty;
      }
      if (pi.pickupIndicator == "D")
        this.cboPickupType.Text = "Domestic";
      else
        this.cboPickupType.Text = "International";
    }

    private void ClearAll()
    {
      this.cboSenderId.SelectedIndex = -1;
      this.cboCountry.SelectedIndex = -1;
      this.edtContactName.Text = "";
      this.edtCompanyName.Text = "";
      this.edtAddress1.Text = "";
      this.edtAddress2.Text = "";
      this.edtCity.Text = "";
      this.cboStateProvince.SelectedIndex = -1;
      this.edtPostal.Text = "";
      this.edtTelephone.Text = "";
      this.edtTelephoneExtension.Text = "";
      this.edtTotalPkgs.Text = "";
      this.edtTotalWeight.Text = "";
      this.cboStateProvince.SelectedIndex = -1;
      this.edtPickupDate.Text = "";
      this.edtReadyTime.Text = "";
      this.edtClosingTime.Text = "";
      this.edtDescription.Text = "";
      this.chkResidential.Checked = false;
      this._pickupInfo = new PickupInfo();
    }

    private void cboSenderId_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
      if (comboBox.SelectedValue == null)
        return;
      this.PostProcessSenderCode(comboBox.SelectedValue.ToString());
    }

    private void SetPickupItemSelectedView(PickupInfo.Status status)
    {
      this.panelPickupHistory.Enabled = true;
      this.panelPackageDetails.Enabled = status == PickupInfo.Status.Failed;
      this.panelPickupInfo.Enabled = status == PickupInfo.Status.Failed;
      this.btnClearFields.Enabled = false;
      this.btnCancelPickup.Enabled = status == PickupInfo.Status.Scheduled;
      this.btnDeleteReservation.Enabled = true;
      this.btnSchedulePickup.Enabled = true;
      this.btnPrintReservation.Enabled = true;
      this.btnSubmitReservation.Enabled = status == PickupInfo.Status.Failed;
    }

    private void btnCancelPickup_Click(object sender, EventArgs e)
    {
      string text = GuiData.Languafier.TranslateError(20662);
      string str = GuiData.Languafier.TranslateError(20670);
      MessageBoxButtons messageBoxButtons = MessageBoxButtons.YesNo;
      string caption = str;
      int buttons = (int) messageBoxButtons;
      if (MessageBox.Show(text, caption, (MessageBoxButtons) buttons) != DialogResult.Yes)
        return;
      this.btnCancelPickup.Enabled = false;
      if (this.bwCancelDispatchThread.IsBusy)
        return;
      try
      {
        this.bwCancelDispatchThread.RunWorkerAsync();
      }
      catch (InvalidOperationException ex)
      {
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, ex.Message);
      }
    }

    private void btnDeleteReservation_Click(object sender, EventArgs e)
    {
      Button button = sender as Button;
      string text1 = GuiData.Languafier.TranslateError(20663);
      string caption = GuiData.Languafier.TranslateError(20670);
      MessageBoxButtons buttons1 = MessageBoxButtons.YesNo;
      switch (this.GetStatusForCurrentDataGridRow())
      {
        case PickupInfo.Status.NotSet:
          break;
        case PickupInfo.Status.Scheduled:
          string text2 = GuiData.Languafier.TranslateError(20664);
          MessageBoxButtons buttons2 = MessageBoxButtons.OK;
          int num1 = (int) MessageBox.Show(text2, caption, buttons2);
          break;
        default:
          if (MessageBox.Show(text1, caption, buttons1) != DialogResult.Yes)
            break;
          button.Enabled = false;
          if (GuiData.AppController.ShipEngine.DeletePickupDispatch(this.GetPickupIdForCurrentDataGridRow()))
          {
            this.PopulatePickupHistory();
            break;
          }
          string text3 = GuiData.Languafier.TranslateError(20661);
          MessageBoxButtons buttons3 = MessageBoxButtons.OK;
          int num2 = (int) MessageBox.Show(text3, caption, buttons3);
          break;
      }
    }

    private void btnPrintReservation_Click(object sender, EventArgs e)
    {
      Error error = new Error();
      int num = (int) new frmPickupConfirmationDlg(this._pickupInfo).ShowDialog();
    }

    private void ScreenToPickupInfoObject()
    {
      this._pickupInfo.SenderCode = this.cboSenderId.SelectedValue as string;
      this._pickupInfo.CountryCode = this.cboCountry.SelectedValue as string;
      this._pickupInfo.ContactName = this.edtContactName.Text;
      this._pickupInfo.CompanyName = this.edtCompanyName.Text;
      this._pickupInfo.Address1 = this.edtAddress1.Text;
      this._pickupInfo.Address2 = this.edtAddress2.Text;
      this._pickupInfo.City = this.edtCity.Text;
      this._pickupInfo.StateProvCode = this.cboStateProvince.SelectedValue as string;
      this._pickupInfo.Postal = this.edtPostal.Text.Replace("-", "");
      this._pickupInfo.Phone = this.edtTelephone.Text;
      this._pickupInfo.PhoneExt = this.edtTelephoneExtension.Text;
      this._pickupInfo.TotalPackages = int.Parse(this.edtTotalPkgs.Text);
      this._pickupInfo.TotalWeight = int.Parse(this.edtTotalWeight.Text);
      this._pickupInfo.WeightType = this.cboWeightType.Text;
      this._pickupInfo.PickupDate = Utility.TextToDate(this.edtPickupDate.Text);
      this._pickupInfo.ReadyTime = DateTime.Parse(this.edtReadyTime.Text);
      this._pickupInfo.ClosingTime = DateTime.Parse(this.edtClosingTime.Text);
      this._pickupInfo.Remarks = this.edtDescription.Text;
      this._pickupInfo.MeterNbr = this._meter;
      this._pickupInfo.FedExAccountNbr = this._account;
      this._pickupInfo.LocationType = !this.chkResidential.Checked ? "C" : "R";
      if (this.cboCountry.SelectedValue as string == "MX" && this.cboPickupType.Text == "Domestic")
        this._pickupInfo.pickupIndicator = "D";
      else
        this._pickupInfo.pickupIndicator = "I";
    }

    private void FillTestPickupInfoObject(PickupInfo pi)
    {
      pi.SenderCode = "";
      pi.CountryCode = "MX";
      pi.ContactName = "Don Juan";
      pi.CompanyName = "Frijoles Inc.";
      pi.Address1 = "Dos Equis Drive";
      pi.Address2 = "Puerto Quatro";
      pi.City = "San Carlos";
      pi.StateProvCode = "Sonora";
      pi.Postal = "21345";
      pi.Phone = "43423434343";
      pi.PhoneExt = "111111";
      pi.TotalPackages = 23;
      pi.TotalWeight = 500;
      pi.WeightType = "KGS";
      pi.PickupDate = DateTime.Today;
      pi.ReadyTime = DateTime.Today;
      pi.ClosingTime = DateTime.Today;
      pi.Remarks = "My remarks";
      pi.MeterNbr = "48048";
      pi.FedExAccountNbr = "020054611";
      pi.OpCo = (short) 1;
      pi.LocationType = "R";
    }

    private void btnSubmitReservation_Click(object sender, EventArgs e)
    {
      if (!this.ValidatePickupFields())
        return;
      this.ScreenToPickupInfoObject();
      PickupInfo pickupInfo = new PickupInfo();
      Error error = new Error();
      int num = (int) MessageBox.Show(GuiData.Languafier.TranslateError(20666));
      this.btnSubmitReservation.Enabled = false;
      if (this.bwDispatchThread.IsBusy)
        return;
      try
      {
        this.bwDispatchThread.RunWorkerAsync();
      }
      catch (InvalidOperationException ex)
      {
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, ex.Message);
      }
    }

    public static string GetErrorMessageFromErrorCode(string errCode)
    {
      string messageFromErrorCode = string.Empty;
      int result = 0;
      if (string.IsNullOrEmpty(messageFromErrorCode) && int.TryParse(errCode, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        messageFromErrorCode = GuiData.Languafier.TranslateError(result);
      if (string.IsNullOrEmpty(messageFromErrorCode) && int.TryParse(errCode, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        messageFromErrorCode = GuiData.Languafier.TranslateError(result);
      if (string.IsNullOrEmpty(messageFromErrorCode))
        messageFromErrorCode = "Unknown Error";
      return messageFromErrorCode;
    }

    private string GetPickupIdForCurrentDataGridRow()
    {
      return (string) this.dgPickupHistory.CurrentRow.Cells["PickupId"].Value;
    }

    private PickupInfo.Status GetStatusForCurrentDataGridRow()
    {
      DataGridViewRow currentRow = this.dgPickupHistory.CurrentRow;
      return currentRow != null ? (PickupInfo.Status) currentRow.Cells["Status"].Tag : PickupInfo.Status.NotSet;
    }

    private void cboCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
      try
      {
        if (comboBox == null || comboBox.SelectedValue == null || Convert.IsDBNull(comboBox.SelectedValue))
          return;
        this.SetMasks(comboBox.SelectedValue.ToString());
        Utility.PopulateStateProvinceCombo(this.cboStateProvince, comboBox.SelectedValue.ToString().Substring(0, 2));
        this.cboStateProvince.SelectedIndex = -1;
        if (comboBox.SelectedValue.ToString().Equals("MX"))
        {
          this.cboPickupType.Visible = true;
          this.lblPickupType.Visible = true;
        }
        else
        {
          this.cboPickupType.Visible = false;
          this.lblPickupType.Visible = false;
        }
      }
      catch (Exception ex)
      {
      }
    }

    private bool ValidatePickupFields()
    {
      bool flag = true;
      string text = string.Empty;
      DateTime result1;
      if (!DateTime.TryParseExact(this.edtPickupDate.Text, Utility.DateFormatString, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result1))
      {
        this.edtPickupDate.Focus();
        text = GuiData.Languafier.TranslateError(20653);
        flag = false;
      }
      else if (!DateTime.TryParse(this.edtReadyTime.Text, out result1))
      {
        this.edtReadyTime.Focus();
        text = GuiData.Languafier.TranslateError(20654);
        flag = false;
      }
      else if (!DateTime.TryParse(this.edtClosingTime.Text, out result1))
      {
        this.edtClosingTime.Focus();
        text = GuiData.Languafier.TranslateError(20655);
        flag = false;
      }
      else
      {
        int result2;
        if (!int.TryParse(this.edtTotalPkgs.Text, out result2) || result2 == 0)
        {
          this.edtTotalPkgs.Focus();
          text = GuiData.Languafier.TranslateError(20656);
          flag = false;
        }
        else if (!int.TryParse(this.edtTotalWeight.Text, out result2) || result2 == 0)
        {
          this.edtTotalWeight.Focus();
          text = GuiData.Languafier.TranslateError(20657);
          flag = false;
        }
        else
        {
          result1 = DateTime.Parse(this.edtReadyTime.Text);
          DateTime dateTime = DateTime.Parse(this.edtClosingTime.Text);
          if (result1 > dateTime)
          {
            text = GuiData.Languafier.TranslateError(20658);
            flag = false;
          }
        }
      }
      if (!flag)
      {
        int num = (int) MessageBox.Show(text);
      }
      return flag;
    }

    private void bwDispatchThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        int num1 = (int) MessageBox.Show(e.Error.Message);
      }
      else
      {
        string text;
        if ((bool) e.Result)
        {
          this.panelMessage.Visible = false;
          this.lblMessageText.Text = string.Empty;
          text = GuiData.Languafier.TranslateError(20659);
        }
        else
        {
          text = GuiData.Languafier.TranslateError(20660);
          this.panelMessage.Visible = true;
          this.lblMessageText.Text = PickupView.GetErrorMessageFromErrorCode(this._pickupInfo.ErrorMessage);
        }
        int num2 = (int) MessageBox.Show(text);
        this.PopulatePickupHistory();
      }
      this.btnSubmitReservation.Enabled = true;
    }

    private void bwDispatchThread_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      e.Result = (object) this.DoMe(worker, e);
    }

    private void bwCancelDispatchThread_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      e.Result = (object) this.DoMe2(worker, e);
    }

    private void bwCancelDispatchThread_RunWorkerCompleted(
      object sender,
      RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        int num1 = (int) MessageBox.Show(e.Error.Message);
      }
      else if (!(bool) e.Result)
      {
        MessageBoxButtons buttons = MessageBoxButtons.OK;
        int num2 = (int) MessageBox.Show(GuiData.Languafier.TranslateError(20669), GuiData.Languafier.TranslateError(20670), buttons);
      }
      this.PopulatePickupHistory();
      this.btnCancelPickup.Enabled = true;
    }

    private bool DoMe(BackgroundWorker worker, DoWorkEventArgs e)
    {
      Error puError = new Error();
      PickupInfo outPickupInfo = new PickupInfo();
      bool flag = false;
      try
      {
        flag = GuiData.AppController.ShipEngine.SubmitPickup(this._pickupInfo, out outPickupInfo, puError);
        this._pickupInfo.ErrorMessage = outPickupInfo.ErrorMessage;
        this._pickupInfo.ReservationNbr = outPickupInfo.ReservationNbr;
        this._pickupInfo.DispatchLocId = outPickupInfo.DispatchLocId;
      }
      catch (Exception ex)
      {
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, ex.Message);
      }
      return flag;
    }

    private bool DoMe2(BackgroundWorker worker, DoWorkEventArgs e)
    {
      Error error = new Error();
      bool flag = false;
      try
      {
        flag = GuiData.AppController.ShipEngine.CancelPickup(this._pickupInfo, error);
      }
      catch (Exception ex)
      {
        FxLogger.LogMessage(FedEx.Gsm.Common.Logging.LogLevel.Error, FxLogger.AppCode_PickUp, this.ToString() + "." + MethodBase.GetCurrentMethod().Name, ex.Message);
      }
      return flag;
    }

    private void dgPickupHistory_ColumnHeaderMouseClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      FedEx.Gsm.Common.ConfigManager.ConfigManager configManager = new FedEx.Gsm.Common.ConfigManager.ConfigManager(FedEx.Gsm.Common.ConfigManager.ConfigManager.Sections.GUI);
      configManager.SetProfileValue("SETTINGS/PICKUP", "SortByColumn", (object) e.ColumnIndex);
      configManager.SetProfileValue("SETTINGS/PICKUP", "SortOrder", (object) this.dgPickupHistory.SortOrder);
    }

    private void dgPickupHistory_SelectionChanged(object sender, EventArgs e)
    {
      Error error = new Error();
      if (this.dgPickupHistory.SelectedRows == null || this.dgPickupHistory.SelectedRows.Count != 1)
        return;
      this._pickupInfo = GuiData.AppController.ShipEngine.GetPickupInfoForPickupId((string) this.dgPickupHistory.SelectedRows[0].Cells["PickupId"].Value, error);
      this.PickupInfoObjectToScreen(this._pickupInfo);
      this.SetPickupItemSelectedView(this._pickupInfo.StatusCode);
    }

    private void dgPickupHistory_CellFormatting(
      object sender,
      DataGridViewCellFormattingEventArgs e)
    {
      string name = this.dgPickupHistory.Columns[e.ColumnIndex].Name;
      if (string.IsNullOrEmpty(name) || !(name == "PickupType"))
        return;
      if (e.Value.ToString() == "D")
        e.Value = (object) "Domestic";
      else
        e.Value = (object) "International";
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PickupView));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.panelPickupHistory = new Panel();
      this.dgPickupHistory = new DataGridView();
      this.ReservationNbr = new DataGridViewTextBoxColumn();
      this.PickupDate = new DataGridViewTextBoxColumn();
      this.PickupTime = new DataGridViewTextBoxColumn();
      this.ContactName = new DataGridViewTextBoxColumn();
      this.Status = new DataGridViewTextBoxColumn();
      this.PickupId = new DataGridViewTextBoxColumn();
      this.PickupType = new DataGridViewTextBoxColumn();
      this.panelMessage = new Panel();
      this.lblMessageText = new Label();
      this.lblMessageLeft = new Label();
      this.lblMessageRight = new Label();
      this.lblMessageBottom = new Label();
      this.lblMessageTop = new Label();
      this.panelPickupInfo = new Panel();
      this.chkResidential = new CheckBoxEx();
      this.edtTelephone = new FdxMaskedEdit();
      this.lblSenderId = new Label();
      this.edtPostal = new FdxMaskedEdit();
      this.edtAddress2 = new TextBox();
      this.lblCountry = new Label();
      this.edtAddress1 = new TextBox();
      this.lblTelephoneExtension = new Label();
      this.lblContact = new Label();
      this.edtCompanyName = new TextBox();
      this.lblState = new Label();
      this.lblCompanyName = new Label();
      this.edtContactName = new TextBox();
      this.lblTelephone = new Label();
      this.lblAddress1 = new Label();
      this.cboCountry = new ComboBox();
      this.lblCity = new Label();
      this.lblAddress2 = new Label();
      this.cboSenderId = new ComboBox();
      this.lblPostal = new Label();
      this.cboStateProvince = new ComboBox();
      this.edtCity = new ComboBox();
      this.edtTelephoneExtension = new FdxMaskedEdit();
      this.panelPackageDetails = new Panel();
      this.cboPickupType = new ComboBox();
      this.lblPickupType = new Label();
      this.edtPickupDate = new FdxMaskedEdit();
      this.btnCalendar = new Button();
      this.imageList1 = new ImageList();
      this.edtDescription = new TextBox();
      this.edtClosingTime = new FdxMaskedEdit();
      this.edtReadyTime = new FdxMaskedEdit();
      this.lblDescription = new Label();
      this.lblClosingTime = new Label();
      this.lblReadyTime = new Label();
      this.lblPickupDate = new Label();
      this.cboWeightType = new ComboBox();
      this.edtTotalWeight = new FdxMaskedEdit();
      this.edtTotalPkgs = new FdxMaskedEdit();
      this.lblTotalWeight = new Label();
      this.lblTtlPkgs = new Label();
      this.panelBottomButtons = new Panel();
      this.btnSubmitReservation = new Button();
      this.btnPrintReservation = new Button();
      this.btnCancelPickup = new Button();
      this.btnSchedulePickup = new Button();
      this.btnDeleteReservation = new Button();
      this.btnClearFields = new Button();
      this.htPickupHistory = new HeaderText();
      this.htPackageDetails = new HeaderText();
      this.htPickupInfo = new HeaderText();
      this.verticalLine = new PictureBox();
      this.bwDispatchThread = new BackgroundWorker();
      this.bwCancelDispatchThread = new BackgroundWorker();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
      this.panelPickupHistory.SuspendLayout();
      ((ISupportInitialize) this.dgPickupHistory).BeginInit();
      this.panelMessage.SuspendLayout();
      this.panelPickupInfo.SuspendLayout();
      this.panelPackageDetails.SuspendLayout();
      this.panelBottomButtons.SuspendLayout();
      ((ISupportInitialize) this.verticalLine).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.helpProvider1, "helpProvider1");
      this.panelPickupHistory.Controls.Add((Control) this.dgPickupHistory);
      this.panelPickupHistory.Controls.Add((Control) this.panelMessage);
      componentResourceManager.ApplyResources((object) this.panelPickupHistory, "panelPickupHistory");
      this.panelPickupHistory.Name = "panelPickupHistory";
      this.helpProvider1.SetShowHelp((Control) this.panelPickupHistory, (bool) componentResourceManager.GetObject("panelPickupHistory.ShowHelp"));
      this.dgPickupHistory.AllowUserToAddRows = false;
      this.dgPickupHistory.AllowUserToDeleteRows = false;
      componentResourceManager.ApplyResources((object) this.dgPickupHistory, "dgPickupHistory");
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgPickupHistory.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgPickupHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPickupHistory.Columns.AddRange((DataGridViewColumn) this.ReservationNbr, (DataGridViewColumn) this.PickupDate, (DataGridViewColumn) this.PickupTime, (DataGridViewColumn) this.ContactName, (DataGridViewColumn) this.Status, (DataGridViewColumn) this.PickupId, (DataGridViewColumn) this.PickupType);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgPickupHistory.DefaultCellStyle = gridViewCellStyle2;
      this.dgPickupHistory.EditMode = DataGridViewEditMode.EditProgrammatically;
      this.helpProvider1.SetHelpKeyword((Control) this.dgPickupHistory, componentResourceManager.GetString("dgPickupHistory.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.dgPickupHistory, (HelpNavigator) componentResourceManager.GetObject("dgPickupHistory.HelpNavigator"));
      this.dgPickupHistory.MultiSelect = false;
      this.dgPickupHistory.Name = "dgPickupHistory";
      this.dgPickupHistory.ReadOnly = true;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgPickupHistory.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgPickupHistory.RowHeadersVisible = false;
      this.dgPickupHistory.RowTemplate.ReadOnly = true;
      this.dgPickupHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.helpProvider1.SetShowHelp((Control) this.dgPickupHistory, (bool) componentResourceManager.GetObject("dgPickupHistory.ShowHelp"));
      this.dgPickupHistory.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgPickupHistory_CellFormatting);
      this.dgPickupHistory.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgPickupHistory_ColumnHeaderMouseClick);
      this.dgPickupHistory.SelectionChanged += new EventHandler(this.dgPickupHistory_SelectionChanged);
      this.ReservationNbr.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.ReservationNbr.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.ReservationNbr, "ReservationNbr");
      this.ReservationNbr.Name = "ReservationNbr";
      this.ReservationNbr.ReadOnly = true;
      this.ReservationNbr.Resizable = DataGridViewTriState.True;
      this.PickupDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.PickupDate.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.PickupDate, "PickupDate");
      this.PickupDate.Name = "PickupDate";
      this.PickupDate.ReadOnly = true;
      this.PickupTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.PickupTime.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.PickupTime, "PickupTime");
      this.PickupTime.Name = "PickupTime";
      this.PickupTime.ReadOnly = true;
      this.ContactName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.ContactName.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.ContactName, "ContactName");
      this.ContactName.Name = "ContactName";
      this.ContactName.ReadOnly = true;
      this.Status.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Status.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.Status, "Status");
      this.Status.Name = "Status";
      this.Status.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.PickupId, "PickupId");
      this.PickupId.Name = "PickupId";
      this.PickupId.ReadOnly = true;
      this.PickupType.FillWeight = 307.6142f;
      componentResourceManager.ApplyResources((object) this.PickupType, "PickupType");
      this.PickupType.Name = "PickupType";
      this.PickupType.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.panelMessage, "panelMessage");
      this.panelMessage.Controls.Add((Control) this.lblMessageText);
      this.panelMessage.Controls.Add((Control) this.lblMessageLeft);
      this.panelMessage.Controls.Add((Control) this.lblMessageRight);
      this.panelMessage.Controls.Add((Control) this.lblMessageBottom);
      this.panelMessage.Controls.Add((Control) this.lblMessageTop);
      this.helpProvider1.SetHelpKeyword((Control) this.panelMessage, componentResourceManager.GetString("panelMessage.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.panelMessage, (HelpNavigator) componentResourceManager.GetObject("panelMessage.HelpNavigator"));
      this.panelMessage.Name = "panelMessage";
      this.helpProvider1.SetShowHelp((Control) this.panelMessage, (bool) componentResourceManager.GetObject("panelMessage.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblMessageText, "lblMessageText");
      this.lblMessageText.ForeColor = Color.Red;
      this.lblMessageText.Name = "lblMessageText";
      this.helpProvider1.SetShowHelp((Control) this.lblMessageText, (bool) componentResourceManager.GetObject("lblMessageText.ShowHelp"));
      this.lblMessageLeft.BackColor = Color.Red;
      componentResourceManager.ApplyResources((object) this.lblMessageLeft, "lblMessageLeft");
      this.lblMessageLeft.Name = "lblMessageLeft";
      this.helpProvider1.SetShowHelp((Control) this.lblMessageLeft, (bool) componentResourceManager.GetObject("lblMessageLeft.ShowHelp"));
      this.lblMessageRight.BackColor = Color.Red;
      componentResourceManager.ApplyResources((object) this.lblMessageRight, "lblMessageRight");
      this.lblMessageRight.Name = "lblMessageRight";
      this.helpProvider1.SetShowHelp((Control) this.lblMessageRight, (bool) componentResourceManager.GetObject("lblMessageRight.ShowHelp"));
      this.lblMessageBottom.BackColor = Color.Red;
      componentResourceManager.ApplyResources((object) this.lblMessageBottom, "lblMessageBottom");
      this.lblMessageBottom.Name = "lblMessageBottom";
      this.helpProvider1.SetShowHelp((Control) this.lblMessageBottom, (bool) componentResourceManager.GetObject("lblMessageBottom.ShowHelp"));
      this.lblMessageTop.BackColor = Color.Red;
      componentResourceManager.ApplyResources((object) this.lblMessageTop, "lblMessageTop");
      this.lblMessageTop.Name = "lblMessageTop";
      this.helpProvider1.SetShowHelp((Control) this.lblMessageTop, (bool) componentResourceManager.GetObject("lblMessageTop.ShowHelp"));
      this.panelPickupInfo.Controls.Add((Control) this.chkResidential);
      this.panelPickupInfo.Controls.Add((Control) this.edtTelephone);
      this.panelPickupInfo.Controls.Add((Control) this.lblSenderId);
      this.panelPickupInfo.Controls.Add((Control) this.edtPostal);
      this.panelPickupInfo.Controls.Add((Control) this.edtAddress2);
      this.panelPickupInfo.Controls.Add((Control) this.lblCountry);
      this.panelPickupInfo.Controls.Add((Control) this.edtAddress1);
      this.panelPickupInfo.Controls.Add((Control) this.lblTelephoneExtension);
      this.panelPickupInfo.Controls.Add((Control) this.lblContact);
      this.panelPickupInfo.Controls.Add((Control) this.edtCompanyName);
      this.panelPickupInfo.Controls.Add((Control) this.lblState);
      this.panelPickupInfo.Controls.Add((Control) this.lblCompanyName);
      this.panelPickupInfo.Controls.Add((Control) this.edtContactName);
      this.panelPickupInfo.Controls.Add((Control) this.lblTelephone);
      this.panelPickupInfo.Controls.Add((Control) this.lblAddress1);
      this.panelPickupInfo.Controls.Add((Control) this.cboCountry);
      this.panelPickupInfo.Controls.Add((Control) this.lblCity);
      this.panelPickupInfo.Controls.Add((Control) this.lblAddress2);
      this.panelPickupInfo.Controls.Add((Control) this.cboSenderId);
      this.panelPickupInfo.Controls.Add((Control) this.lblPostal);
      this.panelPickupInfo.Controls.Add((Control) this.cboStateProvince);
      this.panelPickupInfo.Controls.Add((Control) this.edtCity);
      this.panelPickupInfo.Controls.Add((Control) this.edtTelephoneExtension);
      componentResourceManager.ApplyResources((object) this.panelPickupInfo, "panelPickupInfo");
      this.panelPickupInfo.Name = "panelPickupInfo";
      this.helpProvider1.SetShowHelp((Control) this.panelPickupInfo, (bool) componentResourceManager.GetObject("panelPickupInfo.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.chkResidential, componentResourceManager.GetString("chkResidential.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.chkResidential, (HelpNavigator) componentResourceManager.GetObject("chkResidential.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.chkResidential, "chkResidential");
      this.chkResidential.Name = "chkResidential";
      this.chkResidential.ReadOnly = false;
      this.helpProvider1.SetShowHelp((Control) this.chkResidential, (bool) componentResourceManager.GetObject("chkResidential.ShowHelp"));
      this.chkResidential.TabStop = false;
      this.chkResidential.UseVisualStyleBackColor = true;
      this.edtTelephone.Allow = "";
      this.edtTelephone.Disallow = "";
      this.edtTelephone.eMask = eMasks.maskPhoneTen;
      this.edtTelephone.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtTelephone, componentResourceManager.GetString("edtTelephone.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtTelephone, (HelpNavigator) componentResourceManager.GetObject("edtTelephone.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtTelephone, "edtTelephone");
      this.edtTelephone.Mask = "(999) 999-9999";
      this.edtTelephone.Name = "edtTelephone";
      this.helpProvider1.SetShowHelp((Control) this.edtTelephone, (bool) componentResourceManager.GetObject("edtTelephone.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblSenderId, "lblSenderId");
      this.lblSenderId.Name = "lblSenderId";
      this.helpProvider1.SetShowHelp((Control) this.lblSenderId, (bool) componentResourceManager.GetObject("lblSenderId.ShowHelp"));
      this.edtPostal.Allow = "";
      this.edtPostal.Disallow = "";
      this.edtPostal.eMask = eMasks.maskZipCodeNine;
      this.edtPostal.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtPostal, componentResourceManager.GetString("edtPostal.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtPostal, (HelpNavigator) componentResourceManager.GetObject("edtPostal.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtPostal, "edtPostal");
      this.edtPostal.Mask = "99999-9999";
      this.edtPostal.Name = "edtPostal";
      this.helpProvider1.SetShowHelp((Control) this.edtPostal, (bool) componentResourceManager.GetObject("edtPostal.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.edtAddress2, componentResourceManager.GetString("edtAddress2.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtAddress2, (HelpNavigator) componentResourceManager.GetObject("edtAddress2.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtAddress2, "edtAddress2");
      this.edtAddress2.Name = "edtAddress2";
      this.helpProvider1.SetShowHelp((Control) this.edtAddress2, (bool) componentResourceManager.GetObject("edtAddress2.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblCountry, "lblCountry");
      this.lblCountry.Name = "lblCountry";
      this.helpProvider1.SetShowHelp((Control) this.lblCountry, (bool) componentResourceManager.GetObject("lblCountry.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.edtAddress1, componentResourceManager.GetString("edtAddress1.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtAddress1, (HelpNavigator) componentResourceManager.GetObject("edtAddress1.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtAddress1, "edtAddress1");
      this.edtAddress1.Name = "edtAddress1";
      this.helpProvider1.SetShowHelp((Control) this.edtAddress1, (bool) componentResourceManager.GetObject("edtAddress1.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblTelephoneExtension, "lblTelephoneExtension");
      this.lblTelephoneExtension.Name = "lblTelephoneExtension";
      this.helpProvider1.SetShowHelp((Control) this.lblTelephoneExtension, (bool) componentResourceManager.GetObject("lblTelephoneExtension.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblContact, "lblContact");
      this.lblContact.Name = "lblContact";
      this.helpProvider1.SetShowHelp((Control) this.lblContact, (bool) componentResourceManager.GetObject("lblContact.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.edtCompanyName, componentResourceManager.GetString("edtCompanyName.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtCompanyName, (HelpNavigator) componentResourceManager.GetObject("edtCompanyName.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtCompanyName, "edtCompanyName");
      this.edtCompanyName.Name = "edtCompanyName";
      this.helpProvider1.SetShowHelp((Control) this.edtCompanyName, (bool) componentResourceManager.GetObject("edtCompanyName.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblState, "lblState");
      this.lblState.Name = "lblState";
      this.helpProvider1.SetShowHelp((Control) this.lblState, (bool) componentResourceManager.GetObject("lblState.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblCompanyName, "lblCompanyName");
      this.lblCompanyName.Name = "lblCompanyName";
      this.helpProvider1.SetShowHelp((Control) this.lblCompanyName, (bool) componentResourceManager.GetObject("lblCompanyName.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.edtContactName, componentResourceManager.GetString("edtContactName.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtContactName, (HelpNavigator) componentResourceManager.GetObject("edtContactName.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtContactName, "edtContactName");
      this.edtContactName.Name = "edtContactName";
      this.helpProvider1.SetShowHelp((Control) this.edtContactName, (bool) componentResourceManager.GetObject("edtContactName.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblTelephone, "lblTelephone");
      this.lblTelephone.Name = "lblTelephone";
      this.helpProvider1.SetShowHelp((Control) this.lblTelephone, (bool) componentResourceManager.GetObject("lblTelephone.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblAddress1, "lblAddress1");
      this.lblAddress1.Name = "lblAddress1";
      this.helpProvider1.SetShowHelp((Control) this.lblAddress1, (bool) componentResourceManager.GetObject("lblAddress1.ShowHelp"));
      this.cboCountry.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.cboCountry.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cboCountry.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCountry.FormattingEnabled = true;
      this.helpProvider1.SetHelpKeyword((Control) this.cboCountry, componentResourceManager.GetString("cboCountry.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.cboCountry, (HelpNavigator) componentResourceManager.GetObject("cboCountry.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.cboCountry, "cboCountry");
      this.cboCountry.Name = "cboCountry";
      this.helpProvider1.SetShowHelp((Control) this.cboCountry, (bool) componentResourceManager.GetObject("cboCountry.ShowHelp"));
      this.cboCountry.SelectedIndexChanged += new EventHandler(this.cboCountry_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.lblCity, "lblCity");
      this.lblCity.Name = "lblCity";
      this.helpProvider1.SetShowHelp((Control) this.lblCity, (bool) componentResourceManager.GetObject("lblCity.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblAddress2, "lblAddress2");
      this.lblAddress2.Name = "lblAddress2";
      this.helpProvider1.SetShowHelp((Control) this.lblAddress2, (bool) componentResourceManager.GetObject("lblAddress2.ShowHelp"));
      this.cboSenderId.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSenderId.FormattingEnabled = true;
      this.helpProvider1.SetHelpKeyword((Control) this.cboSenderId, componentResourceManager.GetString("cboSenderId.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.cboSenderId, (HelpNavigator) componentResourceManager.GetObject("cboSenderId.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.cboSenderId, "cboSenderId");
      this.cboSenderId.Name = "cboSenderId";
      this.helpProvider1.SetShowHelp((Control) this.cboSenderId, (bool) componentResourceManager.GetObject("cboSenderId.ShowHelp"));
      this.cboSenderId.SelectionChangeCommitted += new EventHandler(this.cboSenderId_SelectionChangeCommitted);
      componentResourceManager.ApplyResources((object) this.lblPostal, "lblPostal");
      this.lblPostal.Name = "lblPostal";
      this.helpProvider1.SetShowHelp((Control) this.lblPostal, (bool) componentResourceManager.GetObject("lblPostal.ShowHelp"));
      this.cboStateProvince.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStateProvince.DropDownWidth = 155;
      this.cboStateProvince.FormattingEnabled = true;
      this.helpProvider1.SetHelpKeyword((Control) this.cboStateProvince, componentResourceManager.GetString("cboStateProvince.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.cboStateProvince, (HelpNavigator) componentResourceManager.GetObject("cboStateProvince.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.cboStateProvince, "cboStateProvince");
      this.cboStateProvince.Name = "cboStateProvince";
      this.helpProvider1.SetShowHelp((Control) this.cboStateProvince, (bool) componentResourceManager.GetObject("cboStateProvince.ShowHelp"));
      this.edtCity.FormattingEnabled = true;
      this.helpProvider1.SetHelpKeyword((Control) this.edtCity, componentResourceManager.GetString("edtCity.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtCity, (HelpNavigator) componentResourceManager.GetObject("edtCity.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtCity, "edtCity");
      this.edtCity.Name = "edtCity";
      this.helpProvider1.SetShowHelp((Control) this.edtCity, (bool) componentResourceManager.GetObject("edtCity.ShowHelp"));
      this.edtTelephoneExtension.Allow = "";
      this.edtTelephoneExtension.Disallow = "";
      this.edtTelephoneExtension.eMask = eMasks.maskCustom;
      this.edtTelephoneExtension.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtTelephoneExtension, componentResourceManager.GetString("edtTelephoneExtension.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtTelephoneExtension, (HelpNavigator) componentResourceManager.GetObject("edtTelephoneExtension.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtTelephoneExtension, "edtTelephoneExtension");
      this.edtTelephoneExtension.Mask = "999999";
      this.edtTelephoneExtension.Name = "edtTelephoneExtension";
      this.helpProvider1.SetShowHelp((Control) this.edtTelephoneExtension, (bool) componentResourceManager.GetObject("edtTelephoneExtension.ShowHelp"));
      this.panelPackageDetails.Controls.Add((Control) this.cboPickupType);
      this.panelPackageDetails.Controls.Add((Control) this.lblPickupType);
      this.panelPackageDetails.Controls.Add((Control) this.edtPickupDate);
      this.panelPackageDetails.Controls.Add((Control) this.btnCalendar);
      this.panelPackageDetails.Controls.Add((Control) this.edtDescription);
      this.panelPackageDetails.Controls.Add((Control) this.edtClosingTime);
      this.panelPackageDetails.Controls.Add((Control) this.edtReadyTime);
      this.panelPackageDetails.Controls.Add((Control) this.lblDescription);
      this.panelPackageDetails.Controls.Add((Control) this.lblClosingTime);
      this.panelPackageDetails.Controls.Add((Control) this.lblReadyTime);
      this.panelPackageDetails.Controls.Add((Control) this.lblPickupDate);
      this.panelPackageDetails.Controls.Add((Control) this.cboWeightType);
      this.panelPackageDetails.Controls.Add((Control) this.edtTotalWeight);
      this.panelPackageDetails.Controls.Add((Control) this.edtTotalPkgs);
      this.panelPackageDetails.Controls.Add((Control) this.lblTotalWeight);
      this.panelPackageDetails.Controls.Add((Control) this.lblTtlPkgs);
      componentResourceManager.ApplyResources((object) this.panelPackageDetails, "panelPackageDetails");
      this.panelPackageDetails.Name = "panelPackageDetails";
      this.helpProvider1.SetShowHelp((Control) this.panelPackageDetails, (bool) componentResourceManager.GetObject("panelPackageDetails.ShowHelp"));
      this.cboPickupType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPickupType.FormattingEnabled = true;
      this.helpProvider1.SetHelpKeyword((Control) this.cboPickupType, componentResourceManager.GetString("cboPickupType.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.cboPickupType, (HelpNavigator) componentResourceManager.GetObject("cboPickupType.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.cboPickupType, "cboPickupType");
      this.cboPickupType.Name = "cboPickupType";
      this.helpProvider1.SetShowHelp((Control) this.cboPickupType, (bool) componentResourceManager.GetObject("cboPickupType.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblPickupType, "lblPickupType");
      this.lblPickupType.Name = "lblPickupType";
      this.helpProvider1.SetShowHelp((Control) this.lblPickupType, (bool) componentResourceManager.GetObject("lblPickupType.ShowHelp"));
      this.edtPickupDate.Allow = "";
      this.edtPickupDate.Disallow = "";
      this.edtPickupDate.eMask = eMasks.maskDate;
      this.edtPickupDate.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtPickupDate, componentResourceManager.GetString("edtPickupDate.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtPickupDate, (HelpNavigator) componentResourceManager.GetObject("edtPickupDate.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtPickupDate, "edtPickupDate");
      this.edtPickupDate.Mask = "99/99/9999";
      this.edtPickupDate.Name = "edtPickupDate";
      this.helpProvider1.SetShowHelp((Control) this.edtPickupDate, (bool) componentResourceManager.GetObject("edtPickupDate.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.btnCalendar, componentResourceManager.GetString("btnCalendar.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.btnCalendar, (HelpNavigator) componentResourceManager.GetObject("btnCalendar.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.btnCalendar, "btnCalendar");
      this.btnCalendar.ImageList = this.imageList1;
      this.btnCalendar.Name = "btnCalendar";
      this.helpProvider1.SetShowHelp((Control) this.btnCalendar, (bool) componentResourceManager.GetObject("btnCalendar.ShowHelp"));
      this.btnCalendar.UseVisualStyleBackColor = true;
      this.btnCalendar.Click += new EventHandler(this.btnCalendar_Click);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "calendar.png");
      this.helpProvider1.SetHelpKeyword((Control) this.edtDescription, componentResourceManager.GetString("edtDescription.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtDescription, (HelpNavigator) componentResourceManager.GetObject("edtDescription.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtDescription, "edtDescription");
      this.edtDescription.Name = "edtDescription";
      this.helpProvider1.SetShowHelp((Control) this.edtDescription, (bool) componentResourceManager.GetObject("edtDescription.ShowHelp"));
      this.edtClosingTime.Allow = "";
      this.edtClosingTime.Disallow = "";
      this.edtClosingTime.eMask = eMasks.maskTime;
      this.edtClosingTime.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtClosingTime, componentResourceManager.GetString("edtClosingTime.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtClosingTime, (HelpNavigator) componentResourceManager.GetObject("edtClosingTime.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtClosingTime, "edtClosingTime");
      this.edtClosingTime.Mask = "99:99";
      this.edtClosingTime.Name = "edtClosingTime";
      this.helpProvider1.SetShowHelp((Control) this.edtClosingTime, (bool) componentResourceManager.GetObject("edtClosingTime.ShowHelp"));
      this.edtReadyTime.Allow = "";
      this.edtReadyTime.Disallow = "";
      this.edtReadyTime.eMask = eMasks.maskTime;
      this.edtReadyTime.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtReadyTime, componentResourceManager.GetString("edtReadyTime.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtReadyTime, (HelpNavigator) componentResourceManager.GetObject("edtReadyTime.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtReadyTime, "edtReadyTime");
      this.edtReadyTime.Mask = "99:99";
      this.edtReadyTime.Name = "edtReadyTime";
      this.helpProvider1.SetShowHelp((Control) this.edtReadyTime, (bool) componentResourceManager.GetObject("edtReadyTime.ShowHelp"));
      this.lblDescription.ForeColor = SystemColors.ControlText;
      componentResourceManager.ApplyResources((object) this.lblDescription, "lblDescription");
      this.lblDescription.Name = "lblDescription";
      this.helpProvider1.SetShowHelp((Control) this.lblDescription, (bool) componentResourceManager.GetObject("lblDescription.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblClosingTime, "lblClosingTime");
      this.lblClosingTime.Name = "lblClosingTime";
      this.helpProvider1.SetShowHelp((Control) this.lblClosingTime, (bool) componentResourceManager.GetObject("lblClosingTime.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblReadyTime, "lblReadyTime");
      this.lblReadyTime.Name = "lblReadyTime";
      this.helpProvider1.SetShowHelp((Control) this.lblReadyTime, (bool) componentResourceManager.GetObject("lblReadyTime.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblPickupDate, "lblPickupDate");
      this.lblPickupDate.Name = "lblPickupDate";
      this.helpProvider1.SetShowHelp((Control) this.lblPickupDate, (bool) componentResourceManager.GetObject("lblPickupDate.ShowHelp"));
      this.cboWeightType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboWeightType.FormattingEnabled = true;
      this.helpProvider1.SetHelpKeyword((Control) this.cboWeightType, componentResourceManager.GetString("cboWeightType.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.cboWeightType, (HelpNavigator) componentResourceManager.GetObject("cboWeightType.HelpNavigator"));
      this.cboWeightType.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cboWeightType.Items"),
        (object) componentResourceManager.GetString("cboWeightType.Items1")
      });
      componentResourceManager.ApplyResources((object) this.cboWeightType, "cboWeightType");
      this.cboWeightType.Name = "cboWeightType";
      this.helpProvider1.SetShowHelp((Control) this.cboWeightType, (bool) componentResourceManager.GetObject("cboWeightType.ShowHelp"));
      this.edtTotalWeight.Allow = "";
      this.edtTotalWeight.Disallow = "";
      this.edtTotalWeight.eMask = eMasks.maskCustom;
      this.edtTotalWeight.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtTotalWeight, componentResourceManager.GetString("edtTotalWeight.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtTotalWeight, (HelpNavigator) componentResourceManager.GetObject("edtTotalWeight.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtTotalWeight, "edtTotalWeight");
      this.edtTotalWeight.Mask = "#####.##";
      this.edtTotalWeight.Name = "edtTotalWeight";
      this.helpProvider1.SetShowHelp((Control) this.edtTotalWeight, (bool) componentResourceManager.GetObject("edtTotalWeight.ShowHelp"));
      this.edtTotalPkgs.Allow = "";
      this.edtTotalPkgs.Disallow = "";
      this.edtTotalPkgs.eMask = eMasks.maskCustom;
      this.edtTotalPkgs.FillFrom = LeftRightAlignment.Left;
      this.helpProvider1.SetHelpKeyword((Control) this.edtTotalPkgs, componentResourceManager.GetString("edtTotalPkgs.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.edtTotalPkgs, (HelpNavigator) componentResourceManager.GetObject("edtTotalPkgs.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.edtTotalPkgs, "edtTotalPkgs");
      this.edtTotalPkgs.Mask = "999";
      this.edtTotalPkgs.Name = "edtTotalPkgs";
      this.helpProvider1.SetShowHelp((Control) this.edtTotalPkgs, (bool) componentResourceManager.GetObject("edtTotalPkgs.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblTotalWeight, "lblTotalWeight");
      this.lblTotalWeight.Name = "lblTotalWeight";
      this.helpProvider1.SetShowHelp((Control) this.lblTotalWeight, (bool) componentResourceManager.GetObject("lblTotalWeight.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.lblTtlPkgs, "lblTtlPkgs");
      this.lblTtlPkgs.Name = "lblTtlPkgs";
      this.helpProvider1.SetShowHelp((Control) this.lblTtlPkgs, (bool) componentResourceManager.GetObject("lblTtlPkgs.ShowHelp"));
      this.panelBottomButtons.Controls.Add((Control) this.btnSubmitReservation);
      this.panelBottomButtons.Controls.Add((Control) this.btnPrintReservation);
      this.panelBottomButtons.Controls.Add((Control) this.btnCancelPickup);
      this.panelBottomButtons.Controls.Add((Control) this.btnSchedulePickup);
      this.panelBottomButtons.Controls.Add((Control) this.btnDeleteReservation);
      this.panelBottomButtons.Controls.Add((Control) this.btnClearFields);
      componentResourceManager.ApplyResources((object) this.panelBottomButtons, "panelBottomButtons");
      this.panelBottomButtons.Name = "panelBottomButtons";
      this.helpProvider1.SetShowHelp((Control) this.panelBottomButtons, (bool) componentResourceManager.GetObject("panelBottomButtons.ShowHelp"));
      this.helpProvider1.SetHelpKeyword((Control) this.btnSubmitReservation, componentResourceManager.GetString("btnSubmitReservation.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.btnSubmitReservation, (HelpNavigator) componentResourceManager.GetObject("btnSubmitReservation.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.btnSubmitReservation, "btnSubmitReservation");
      this.btnSubmitReservation.Name = "btnSubmitReservation";
      this.helpProvider1.SetShowHelp((Control) this.btnSubmitReservation, (bool) componentResourceManager.GetObject("btnSubmitReservation.ShowHelp"));
      this.btnSubmitReservation.UseVisualStyleBackColor = true;
      this.btnSubmitReservation.Click += new EventHandler(this.btnSubmitReservation_Click);
      this.helpProvider1.SetHelpKeyword((Control) this.btnPrintReservation, componentResourceManager.GetString("btnPrintReservation.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.btnPrintReservation, (HelpNavigator) componentResourceManager.GetObject("btnPrintReservation.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.btnPrintReservation, "btnPrintReservation");
      this.btnPrintReservation.Name = "btnPrintReservation";
      this.helpProvider1.SetShowHelp((Control) this.btnPrintReservation, (bool) componentResourceManager.GetObject("btnPrintReservation.ShowHelp"));
      this.btnPrintReservation.UseVisualStyleBackColor = true;
      this.btnPrintReservation.Click += new EventHandler(this.btnPrintReservation_Click);
      this.helpProvider1.SetHelpKeyword((Control) this.btnCancelPickup, componentResourceManager.GetString("btnCancelPickup.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.btnCancelPickup, (HelpNavigator) componentResourceManager.GetObject("btnCancelPickup.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.btnCancelPickup, "btnCancelPickup");
      this.btnCancelPickup.Name = "btnCancelPickup";
      this.helpProvider1.SetShowHelp((Control) this.btnCancelPickup, (bool) componentResourceManager.GetObject("btnCancelPickup.ShowHelp"));
      this.btnCancelPickup.UseVisualStyleBackColor = true;
      this.btnCancelPickup.Click += new EventHandler(this.btnCancelPickup_Click);
      this.helpProvider1.SetHelpKeyword((Control) this.btnSchedulePickup, componentResourceManager.GetString("btnSchedulePickup.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.btnSchedulePickup, (HelpNavigator) componentResourceManager.GetObject("btnSchedulePickup.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.btnSchedulePickup, "btnSchedulePickup");
      this.btnSchedulePickup.Name = "btnSchedulePickup";
      this.helpProvider1.SetShowHelp((Control) this.btnSchedulePickup, (bool) componentResourceManager.GetObject("btnSchedulePickup.ShowHelp"));
      this.btnSchedulePickup.UseVisualStyleBackColor = true;
      this.btnSchedulePickup.Click += new EventHandler(this.btnSchedulePickup_Click);
      this.helpProvider1.SetHelpKeyword((Control) this.btnDeleteReservation, componentResourceManager.GetString("btnDeleteReservation.HelpKeyword"));
      this.helpProvider1.SetHelpNavigator((Control) this.btnDeleteReservation, (HelpNavigator) componentResourceManager.GetObject("btnDeleteReservation.HelpNavigator"));
      componentResourceManager.ApplyResources((object) this.btnDeleteReservation, "btnDeleteReservation");
      this.btnDeleteReservation.Name = "btnDeleteReservation";
      this.helpProvider1.SetShowHelp((Control) this.btnDeleteReservation, (bool) componentResourceManager.GetObject("btnDeleteReservation.ShowHelp"));
      this.btnDeleteReservation.UseVisualStyleBackColor = true;
      this.btnDeleteReservation.Click += new EventHandler(this.btnDeleteReservation_Click);
      componentResourceManager.ApplyResources((object) this.btnClearFields, "btnClearFields");
      this.btnClearFields.Name = "btnClearFields";
      this.helpProvider1.SetShowHelp((Control) this.btnClearFields, (bool) componentResourceManager.GetObject("btnClearFields.ShowHelp"));
      this.btnClearFields.UseVisualStyleBackColor = true;
      this.btnClearFields.Click += new EventHandler(this.btnClearFields_Click);
      componentResourceManager.ApplyResources((object) this.htPickupHistory, "htPickupHistory");
      this.htPickupHistory.LineHeight = 2;
      this.htPickupHistory.LineHeightMargin = 2;
      this.htPickupHistory.Name = "htPickupHistory";
      this.helpProvider1.SetShowHelp((Control) this.htPickupHistory, (bool) componentResourceManager.GetObject("htPickupHistory.ShowHelp"));
      componentResourceManager.ApplyResources((object) this.htPackageDetails, "htPackageDetails");
      this.htPackageDetails.LineHeight = 2;
      this.htPackageDetails.LineHeightMargin = 2;
      this.htPackageDetails.Name = "htPackageDetails";
      componentResourceManager.ApplyResources((object) this.htPickupInfo, "htPickupInfo");
      this.htPickupInfo.LineHeight = 2;
      this.htPickupInfo.LineHeightMargin = 2;
      this.htPickupInfo.Name = "htPickupInfo";
      this.helpProvider1.SetShowHelp((Control) this.htPickupInfo, (bool) componentResourceManager.GetObject("htPickupInfo.ShowHelp"));
      this.verticalLine.BackColor = Color.FromArgb(77, 20, 140);
      componentResourceManager.ApplyResources((object) this.verticalLine, "verticalLine");
      this.verticalLine.Name = "verticalLine";
      this.helpProvider1.SetShowHelp((Control) this.verticalLine, (bool) componentResourceManager.GetObject("verticalLine.ShowHelp"));
      this.verticalLine.TabStop = false;
      this.bwDispatchThread.DoWork += new DoWorkEventHandler(this.bwDispatchThread_DoWork);
      this.bwDispatchThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bwDispatchThread_RunWorkerCompleted);
      this.bwCancelDispatchThread.DoWork += new DoWorkEventHandler(this.bwCancelDispatchThread_DoWork);
      this.bwCancelDispatchThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bwCancelDispatchThread_RunWorkerCompleted);
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn1.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.True;
      this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn3.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn4.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn5.FillWeight = 58.47715f;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn7.FillWeight = 307.6142f;
      componentResourceManager.ApplyResources((object) this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
      this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Window;
      this.Controls.Add((Control) this.panelBottomButtons);
      this.Controls.Add((Control) this.verticalLine);
      this.Controls.Add((Control) this.htPickupHistory);
      this.Controls.Add((Control) this.htPackageDetails);
      this.Controls.Add((Control) this.htPickupInfo);
      this.Controls.Add((Control) this.panelPackageDetails);
      this.Controls.Add((Control) this.panelPickupInfo);
      this.Controls.Add((Control) this.panelPickupHistory);
      this.Name = nameof (PickupView);
      this.helpProvider1.SetShowHelp((Control) this, (bool) componentResourceManager.GetObject("$this.ShowHelp"));
      this.Load += new EventHandler(this.PickupView_Load);
      this.panelPickupHistory.ResumeLayout(false);
      ((ISupportInitialize) this.dgPickupHistory).EndInit();
      this.panelMessage.ResumeLayout(false);
      this.panelPickupInfo.ResumeLayout(false);
      this.panelPickupInfo.PerformLayout();
      this.panelPackageDetails.ResumeLayout(false);
      this.panelPackageDetails.PerformLayout();
      this.panelBottomButtons.ResumeLayout(false);
      ((ISupportInitialize) this.verticalLine).EndInit();
      this.ResumeLayout(false);
    }
  }
}
