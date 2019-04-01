using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using FMScout.ControlContext;
using Young3.FMSearch.Core.Entities.InGame;
using System.Windows.Media;

namespace FMScout.ViewModel
{
    public class ProfileTeamViewModel : ProfileViewModel
    {
        // groupboxes
        public LabeledHeaderContext generaldetails { get; set; }
        public LabeledHeaderContext financedetails { get; set; }
        public LabeledHeaderContext stadiumdetails { get; set; }

        //buttons
        public ObservableCollection<String> combolistplayersitemsList = new ObservableCollection<String>();
        public object buttonlistplayerscontent { get; set; }
        public bool buttonlistplayersisenabled { get; set; }
        public object combolistplayersitems { get; set; }
        public object combolistplayersselecteditem { get; set; }
        public bool combolistplayersisenabled { get; set; }
        public object buttonliststaffcontent { get; set; }
        public bool buttonliststaffisenabled { get; set; }
        public object buttonhealteamcontent { get; set; }
        public bool buttonhealteamisenabled { get; set; }

        // header
        public String nation { get; set; }

        // general
        public String name { get; set; }
        public LabeledTextBoxContext yearfounded { get; set; }
        public LabeledTextBoxContext national { get; set; }
        public LabeledTextBoxContext status { get; set; }
        public LabeledTextBoxContext maxafclubs { get; set; }
        public LabeledTextBoxContext afclubs { get; set; }
        public LabeledTextBoxContext trainingfacilities { get; set; }
        public LabeledTextBoxContext youthfacilities { get; set; }
        public LabeledTextBoxContext youthacademy { get; set; }
        public LabeledTextBoxContext maxattendance { get; set; }
        public LabeledTextBoxContext averageattendance { get; set; }
        public LabeledTextBoxContext minattendance { get; set; }
        public LabeledTextBoxContext reputation { get; set; }

        // finance
        public LabeledTextBoxContext totaltransfer { get; set; }
        public LabeledTextBoxContext remtransfer { get; set; }
        public LabeledTextBoxContext balance { get; set; }
        public LabeledTextBoxContext totalwage { get; set; }
        public LabeledTextBoxContext usedwage { get; set; }
        public LabeledTextBoxContext revenueavailable { get; set; }

        // stadium
        public String stadiumname { get; set; }
        public String stadiumowner { get; set; }
        public String stadiumlocation { get; set; }
        public String stadiumnearby { get; set; }
        public LabeledTextBoxContext decay { get; set; }
        public LabeledTextBoxContext fieldwidth { get; set; }
        public LabeledTextBoxContext fieldlength { get; set; }
        public LabeledTextBoxContext curcapacity { get; set; }
        public LabeledTextBoxContext seatcapacity { get; set; }
        public LabeledTextBoxContext expcapacity { get; set; }
        public LabeledTextBoxContext usedcapacity { get; set; }

        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;

        protected override void setProfileContext(LabeledTextBoxContext num)
        {
            num.LabelWidth = 110;
            num.LabelHeight = 19;
            num.TextBoxWidth = 80;
            num.TextBoxHeight = 19;
            num.TextBoxAlignment = HorizontalAlignment.Center;
        }

        protected void setProfileContext2(LabeledTextBoxContext num)
        {
            num.LabelWidth = 140;
            num.LabelHeight = 19;
            num.TextBoxWidth = 100;
            num.TextBoxHeight = 19;
            num.TextBoxAlignment = HorizontalAlignment.Center;
        }

        protected void setProfileContext3(LabeledTextBoxContext num)
        {
            num.LabelWidth = 120;
            num.LabelHeight = 19;
            num.TextBoxWidth = 60;
            num.TextBoxHeight = 19;
            num.TextBoxAlignment = HorizontalAlignment.Center;
        }

        public void setProfileViewModel(ref Team team, ref TeamGridViewModel r)
        {
            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            this.SelectionButton = new CheckBoxContext();
            this.SelectionButton.IsChecked = true;

            yearfounded = new LabeledTextBoxContext();
            national = new LabeledTextBoxContext();
            status = new LabeledTextBoxContext();
            maxafclubs = new LabeledTextBoxContext();
            afclubs = new LabeledTextBoxContext();
            trainingfacilities = new LabeledTextBoxContext();
            youthfacilities = new LabeledTextBoxContext();
            youthacademy = new LabeledTextBoxContext();
            maxattendance = new LabeledTextBoxContext();
            averageattendance = new LabeledTextBoxContext();
            minattendance = new LabeledTextBoxContext();
            reputation = new LabeledTextBoxContext();

            totaltransfer = new LabeledTextBoxContext();
            remtransfer = new LabeledTextBoxContext();
            balance = new LabeledTextBoxContext();
            totalwage = new LabeledTextBoxContext();
            usedwage = new LabeledTextBoxContext();
            revenueavailable = new LabeledTextBoxContext();

            decay = new LabeledTextBoxContext();
            fieldwidth = new LabeledTextBoxContext();
            fieldlength = new LabeledTextBoxContext();
            curcapacity = new LabeledTextBoxContext();
            seatcapacity = new LabeledTextBoxContext();
            expcapacity = new LabeledTextBoxContext();
            usedcapacity = new LabeledTextBoxContext();

            this.generaldetails = new LabeledHeaderContext();
            this.financedetails = new LabeledHeaderContext();
            this.stadiumdetails = new LabeledHeaderContext();

            setProfileContext(yearfounded);
            setProfileContext(national);
            setProfileContext(status);
            setProfileContext(maxafclubs);
            setProfileContext(afclubs);
            setProfileContext(trainingfacilities);
            setProfileContext(youthfacilities);
            setProfileContext(youthacademy);
            setProfileContext(maxattendance);
            setProfileContext(averageattendance);
            setProfileContext(minattendance);
            setProfileContext(reputation);

            setProfileContext2(totaltransfer);
            setProfileContext2(remtransfer);
            setProfileContext2(balance);
            setProfileContext2(totalwage);
            setProfileContext2(usedwage);
            setProfileContext2(revenueavailable);

            setProfileContext3(decay);
            setProfileContext3(fieldwidth);
            setProfileContext3(fieldlength);
            setProfileContext3(curcapacity);
            setProfileContext3(seatcapacity);
            setProfileContext3(expcapacity);
            setProfileContext3(usedcapacity);

            setControlValues(ref team, ref r);
            setLocalization();
        }

        public void setControlValues(ref Team team, ref TeamGridViewModel r)
        {
            PreferencesSettings curSettings = settings.curPreferencesSettings;
            ObservableCollection<String> ProfileGenericLabels = globalFuncs.localization.ProfileGenericLabels;

            this.ID = r.ID;
            this.name = team.Club.Name;

            this.SelectionButton.Content = this.name;

            string isNational = ProfileGenericLabels[ScoutLocalization.PG_NO];
            if (globalFuncs.localization.regionsNative.Contains(team.Club.Country.Name)) isNational = ProfileGenericLabels[ScoutLocalization.PG_YES];

            this.buttonlistplayerscontent = ProfileGenericLabels[ScoutLocalization.PG_LISTPLAYERS];
            this.buttonliststaffcontent = ProfileGenericLabels[ScoutLocalization.PG_LISTSTAFF];
            this.buttonhealteamcontent = ProfileGenericLabels[ScoutLocalization.PG_HEALTEAM];
            this.buttonhealteamisenabled = false;
            this.combolistplayersitems = this.combolistplayersitemsList;
            this.combolistplayersisenabled = true;
            this.buttonliststaffisenabled = true;
            this.buttonlistplayersisenabled = true;

            if (isNational.Equals(ProfileGenericLabels[ScoutLocalization.PG_NO]))
            {
                this.combolistplayersitemsList.Add(ProfileGenericLabels[ScoutLocalization.PG_ALLSQUADS]);
                for (int i = 0; i < team.Club.Teams.Count; ++i)
                {
                    if (team.Club.Teams[i].Type != TeamTypeEnum.Empty)
                        this.combolistplayersitemsList.Add(team.Club.Teams[i].Type.ToString() + " " + ProfileGenericLabels[ScoutLocalization.PG_TEAM]);
                }

                if (this.combolistplayersitemsList.Count > 1)
                {
                    if (this.combolistplayersitemsList.Count == 2)
                    {
                        this.combolistplayersitemsList.RemoveAt(0);
                        this.combolistplayersisenabled = false;
                    }
                    this.combolistplayersselecteditem = this.combolistplayersitemsList[0];
                }
            }
            else
            {
                this.buttonlistplayersisenabled = false;
                this.combolistplayersitemsList.Add(ProfileGenericLabels[ScoutLocalization.PG_NATIONPLAYERS]);
            }
            this.combolistplayersselecteditem = this.combolistplayersitemsList[0];

            if (settings.curPreferencesSettings.editing.Equals(ProfileGenericLabels[ScoutLocalization.PG_YES]))
            {
                if (this.combolistplayersitemsList.Count > 1 && isNational.Equals(ProfileGenericLabels[ScoutLocalization.PG_NO]))
                    this.buttonhealteamisenabled = true;
            }

            if (isNational.Equals(ProfileGenericLabels[ScoutLocalization.PG_NO]))
            {
                this.nation = team.Club.Country.Name;
                this.nation += " (" + team.Club.Country.Continent.Name + ")";
                this.yearfounded.TextBoxText = team.Club.YearFounded.ToString();
                this.maxattendance.TextBoxText = team.Club.MaximumAttendance.ToString();
                this.averageattendance.TextBoxText = team.Club.AverageAttendance.ToString();
                this.minattendance.TextBoxText = team.Club.MinimumAttendance.ToString();

                this.totaltransfer.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBUDGET];
                this.remtransfer.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBUDGET];
                this.totalwage.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBUDGET];
                this.usedwage.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBUDGET];

                // finance details
                if (team.Club.Finances.RemainingTransferBudget != 29476012)
                {
                    this.totaltransfer.TextBoxText = (team.Club.Finances.SeasonTransferBudget * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);
                    this.remtransfer.TextBoxText = (team.Club.Finances.RemainingTransferBudget * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);
                    this.totalwage.TextBoxText = (team.Club.Finances.WageBudget * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);
                    this.usedwage.TextBoxText = (team.Club.Finances.UsedWage * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);
                }

                this.balance.TextBoxText = (team.Club.Finances.Balance * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);

                if (team.Club.Finances.TransferRevenueMadeAvailable != 0)
                    this.revenueavailable.TextBoxText = (team.Club.Finances.TransferRevenueMadeAvailable * 0.01f).ToString("P0");
                else
                    this.revenueavailable.TextBoxText = "0%";

                // stadium details
                this.stadiumname = team.Stadium.ToString();
                if (team.Stadium.Owner != null)
                    this.stadiumowner = team.Stadium.Owner.Name.ToString();
                else
                    this.stadiumowner = ProfileGenericLabels[ScoutLocalization.PG_NOOWNER];
                this.stadiumlocation = team.Stadium.City.Name.ToString();
                if (team.Stadium.NearbyStadium != null)
                    this.stadiumnearby = team.Stadium.NearbyStadium.Name;
                else
                    this.stadiumnearby = ProfileGenericLabels[ScoutLocalization.PG_NONEARBYSTADIUM];
                this.decay.TextBoxText = team.Stadium.Decay.ToString();
                this.fieldwidth.TextBoxText = (float.Parse(team.Stadium.FieldWidth.ToString())).ToString() + "m";
                this.fieldlength.TextBoxText = (float.Parse(team.Stadium.FieldLength.ToString())).ToString() + "m";
                this.curcapacity.TextBoxText = team.Stadium.StadiumCapacity.ToString();
                this.seatcapacity.TextBoxText = team.Stadium.SeatingCapacity.ToString();
                this.expcapacity.TextBoxText = team.Stadium.ExpansionCapacity.ToString();
                this.usedcapacity.TextBoxText = team.Stadium.UsedCapacity.ToString();
            }
            else
            {
                this.nation = team.Club.Country.Name;
                this.yearfounded.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NONE];
                this.maxattendance.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NONE];
                this.averageattendance.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NONE];
                this.minattendance.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NONE];

                this.totaltransfer.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.remtransfer.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.balance.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.totalwage.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.usedwage.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.revenueavailable.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];

                this.stadiumname = ProfileGenericLabels[ScoutLocalization.PG_NONE];
                this.stadiumowner = ProfileGenericLabels[ScoutLocalization.PG_NOOWNER];
                this.stadiumlocation = ProfileGenericLabels[ScoutLocalization.PG_NOLOCATION];
                this.stadiumnearby = ProfileGenericLabels[ScoutLocalization.PG_NONEARBYSTADIUM];
                this.decay.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE]; ;
                this.fieldwidth.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.fieldlength.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.curcapacity.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.seatcapacity.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.expcapacity.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
                this.usedcapacity.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
            }

            this.national.TextBoxText = isNational;
            this.status.TextBoxText = team.Club.Status.ToString();
            this.maxafclubs.TextBoxText = team.Club.MaxAffiliatedClubNumber.ToString();
            this.afclubs.TextBoxText = team.Club.NumberOfAffiliatedClubs.ToString();
            this.trainingfacilities.TextBoxText = team.Club.TrainingFacilities.ToString();
            this.youthfacilities.TextBoxText = team.Club.YouthFacilities.ToString();
            if (team.Club.YouthAcademy == 0)
                this.youthacademy.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NO];
            else if (team.Club.YouthAcademy == 1)
                this.youthacademy.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_YES];
            else
                this.youthacademy.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_UNAVAILABLE];
            this.reputation.TextBoxText = team.Reputation.ToString();
        }

        public void setLocalization()
        {
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> teamGroupBoxes = localization.teamGroupBoxes;

            int index = -1;
            this.generaldetails.Header = teamGroupBoxes[++index];
            this.financedetails.Header = teamGroupBoxes[++index];
            this.stadiumdetails.Header = teamGroupBoxes[++index];

            ObservableCollection<String> TeamProfileLabels = localization.TeamProfileLabels;
            index = -1;
            this.yearfounded.LabelContent = TeamProfileLabels[++index];
            this.national.LabelContent = TeamProfileLabels[++index];
            this.status.LabelContent = TeamProfileLabels[++index];
            this.maxafclubs.LabelContent = TeamProfileLabels[++index];
            this.afclubs.LabelContent = TeamProfileLabels[++index];
            this.trainingfacilities.LabelContent = TeamProfileLabels[++index];
            this.youthfacilities.LabelContent = TeamProfileLabels[++index];
            this.youthacademy.LabelContent = TeamProfileLabels[++index];
            this.maxattendance.LabelContent = TeamProfileLabels[++index];
            this.averageattendance.LabelContent = TeamProfileLabels[++index];
            this.minattendance.LabelContent = TeamProfileLabels[++index];
            this.reputation.LabelContent = TeamProfileLabels[++index];
            this.totaltransfer.LabelContent = TeamProfileLabels[++index];
            this.remtransfer.LabelContent = TeamProfileLabels[++index];
            this.balance.LabelContent = TeamProfileLabels[++index];
            this.totalwage.LabelContent = TeamProfileLabels[++index];
            this.usedwage.LabelContent = TeamProfileLabels[++index];
            this.revenueavailable.LabelContent = TeamProfileLabels[++index];
            this.decay.LabelContent = TeamProfileLabels[++index];
            this.fieldwidth.LabelContent = TeamProfileLabels[++index];
            this.fieldlength.LabelContent = TeamProfileLabels[++index];
            this.curcapacity.LabelContent = TeamProfileLabels[++index];
            this.seatcapacity.LabelContent = TeamProfileLabels[++index];
            this.expcapacity.LabelContent = TeamProfileLabels[++index];
            this.usedcapacity.LabelContent = TeamProfileLabels[++index];
        }
    }
}