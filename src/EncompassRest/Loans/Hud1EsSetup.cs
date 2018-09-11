using System;
using System.Collections.Generic;
using EncompassRest.Loans.Enums;
using EncompassRest.Schema;

namespace EncompassRest.Loans
{
    /// <summary>
    /// Hud1EsSetup
    /// </summary>
    [Entity(SerializeWholeListWhenDirty = true)]
    public sealed partial class Hud1EsSetup : DirtyExtensibleObject, IIdentifiable
    {
        private DirtyValue<bool?> _annualFeePrepaid;
        /// <summary>
        /// Escrow Setup Annual Fee Prepaid [HUDNN58]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Annual Fee Prepaid")]
        public bool? AnnualFeePrepaid { get => _annualFeePrepaid; set => SetField(ref _annualFeePrepaid, value); }
        private DirtyValue<int?> _annualFees;
        /// <summary>
        /// Escrow Setup Annual Fees [HUDNN21]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Annual Fees")]
        public int? AnnualFees { get => _annualFees; set => SetField(ref _annualFees, value); }
        private DirtyValue<string> _date;
        /// <summary>
        /// Escrow Setup Date [HUDNN12]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Date")]
        public string Date { get => _date; set => SetField(ref _date, value); }
        private DirtyValue<int?> _floodInsDisb;
        /// <summary>
        /// Escrow Setup Flood Ins Disb [HUDNN16]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Flood Ins Disb")]
        public int? FloodInsDisb { get => _floodInsDisb; set => SetField(ref _floodInsDisb, value); }
        private DirtyValue<bool?> _floodInsPrepaid;
        /// <summary>
        /// Escrow Setup Flood Ins Prepaid [HUDNN53]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Flood Ins Prepaid")]
        public bool? FloodInsPrepaid { get => _floodInsPrepaid; set => SetField(ref _floodInsPrepaid, value); }
        private DirtyValue<int?> _hazInsDisb;
        /// <summary>
        /// Escrow Setup Haz Ins Disb [HUDNN14]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Haz Ins Disb")]
        public int? HazInsDisb { get => _hazInsDisb; set => SetField(ref _hazInsDisb, value); }
        private DirtyValue<bool?> _hazInsPrepaid;
        /// <summary>
        /// Escrow Setup Haz Ins Prepaid [HUDNN51]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Haz Ins Prepaid")]
        public bool? HazInsPrepaid { get => _hazInsPrepaid; set => SetField(ref _hazInsPrepaid, value); }
        private DirtyValue<int?> _hud1EsSetupIndex;
        /// <summary>
        /// Hud1EsSetup Hud1EsSetupIndex
        /// </summary>
        public int? Hud1EsSetupIndex { get => _hud1EsSetupIndex; set => SetField(ref _hud1EsSetupIndex, value); }
        private DirtyValue<string> _id;
        /// <summary>
        /// Hud1EsSetup Id
        /// </summary>
        public string Id { get => _id; set => SetField(ref _id, value); }
        private DirtyValue<int?> _mtgInsDisb;
        /// <summary>
        /// Escrow Setup Mtg Ins Disb [HUDNN15]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Mtg Ins Disb")]
        public int? MtgInsDisb { get => _mtgInsDisb; set => SetField(ref _mtgInsDisb, value); }
        private DirtyValue<bool?> _mtgInsPrepaid;
        /// <summary>
        /// Escrow Setup Mtg Ins Prepaid [HUDNN52]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Mtg Ins Prepaid")]
        public bool? MtgInsPrepaid { get => _mtgInsPrepaid; set => SetField(ref _mtgInsPrepaid, value); }
        private DirtyValue<int?> _schoolTaxes;
        /// <summary>
        /// Escrow Setup City Taxes [HUDNN17]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup City Taxes")]
        public int? SchoolTaxes { get => _schoolTaxes; set => SetField(ref _schoolTaxes, value); }
        private DirtyValue<bool?> _schoolTaxesPrepaid;
        /// <summary>
        /// Escrow Setup City Taxes Prepaid [HUDNN54]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup City Taxes Prepaid")]
        public bool? SchoolTaxesPrepaid { get => _schoolTaxesPrepaid; set => SetField(ref _schoolTaxesPrepaid, value); }
        private DirtyValue<int?> _taxDisb;
        /// <summary>
        /// Escrow Setup Tax Disb [HUDNN13]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Tax Disb")]
        public int? TaxDisb { get => _taxDisb; set => SetField(ref _taxDisb, value); }
        private DirtyValue<bool?> _taxPrepaid;
        /// <summary>
        /// Escrow Setup Tax Prepaid [HUDNN50]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup Tax Prepaid")]
        public bool? TaxPrepaid { get => _taxPrepaid; set => SetField(ref _taxPrepaid, value); }
        private DirtyValue<int?> _userDefined1;
        /// <summary>
        /// Escrow Setup User Defined 1 [HUDNN18]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup User Defined 1")]
        public int? UserDefined1 { get => _userDefined1; set => SetField(ref _userDefined1, value); }
        private DirtyValue<int?> _userDefined2;
        /// <summary>
        /// Escrow Setup User Defined 2 [HUDNN19]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup User Defined 2")]
        public int? UserDefined2 { get => _userDefined2; set => SetField(ref _userDefined2, value); }
        private DirtyValue<int?> _userDefined3;
        /// <summary>
        /// Escrow Setup User Defined 3 [HUDNN20]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup User Defined 3")]
        public int? UserDefined3 { get => _userDefined3; set => SetField(ref _userDefined3, value); }
        private DirtyValue<bool?> _userDefinedPrepaid1;
        /// <summary>
        /// Escrow Setup User Defined 1 Prepaid [HUDNN55]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup User Defined 1 Prepaid")]
        public bool? UserDefinedPrepaid1 { get => _userDefinedPrepaid1; set => SetField(ref _userDefinedPrepaid1, value); }
        private DirtyValue<bool?> _userDefinedPrepaid2;
        /// <summary>
        /// Escrow Setup User Defined 2 Prepaid [HUDNN56]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup User Defined 2 Prepaid")]
        public bool? UserDefinedPrepaid2 { get => _userDefinedPrepaid2; set => SetField(ref _userDefinedPrepaid2, value); }
        private DirtyValue<bool?> _userDefinedPrepaid3;
        /// <summary>
        /// Escrow Setup User Defined 3 Prepaid [HUDNN57]
        /// </summary>
        [LoanFieldProperty(Description = "Escrow Setup User Defined 3 Prepaid")]
        public bool? UserDefinedPrepaid3 { get => _userDefinedPrepaid3; set => SetField(ref _userDefinedPrepaid3, value); }
    }
}