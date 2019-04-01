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
    public class ProfilePlayerViewModel : ProfileViewModel
    {
        // groupboxes
        public LabeledHeaderContext personaldetails { get; set; }
        public LabeledHeaderContext contractdetails { get; set; }
        public LabeledHeaderContext technicalattributes { get; set; }
        public LabeledHeaderContext physicalattributes { get; set; }
        public LabeledHeaderContext mentalattributes { get; set; }
        public LabeledHeaderContext hiddenattributes { get; set; }
        public LabeledHeaderContext otherattributes { get; set; }
        public LabeledHeaderContext goalkeepingattributes { get; set; }
        public LabeledHeaderContext mentaltraitsattributes { get; set; }

        // header
        public LabeledHeaderContext position { get; set; }
        public bool? shortlistchecked { get; set; }
        public LabeledHeaderContext shortlisttext { get; set; }
        public LabeledHeaderContext viewtechnical { get; set; }
        public LabeledHeaderContext viewgoalkeeping { get; set; }

        // personal
        public LabeledHeaderContext name { get; set; }
        public LabeledHeaderContext fullname { get; set; }
        public LabeledHeaderContext nation { get; set; }
        public LabeledHeaderContext formed { get; set; }
        public LabeledHeaderContext eu { get; set; }
        public LabeledHeaderContext countryhg { get; set; }
        public LabeledHeaderContext birthdate { get; set; }
        public LabeledHeaderContext age { get; set; }
        public LabeledHeaderContext height { get; set; }
        public LabeledHeaderContext weight { get; set; }
        public LabeledHeaderContext value { get; set; }
        public LabeledHeaderContext salevalue { get; set; }
        public LabeledHeaderContext international { get; set; }
        public LabeledHeaderContext preffoot { get; set; }
        public LabeledHeaderContext ca { get; set; }
        public LabeledHeaderContext pa { get; set; }
        public LabeledHeaderContext regen { get; set; }

        // contract
        public LabeledHeaderContext club { get; set; }
        public LabeledHeaderContext curclub { get; set; }
        public LabeledTextBoxContext teamsquad { get; set; }
        public LabeledTextBoxContext contractstarted { get; set; }
        public LabeledTextBoxContext contractexpiring { get; set; }
        public LabeledTextBoxContext wage { get; set; }
        public LabeledTextBoxContext appbonus { get; set; }
        public LabeledTextBoxContext goalbonus { get; set; }
        public LabeledTextBoxContext cleanbonus { get; set; }

        public int worldrep { get; set; }
        public int nationalrep { get; set; }
        public int localrep { get; set; }

        // other
        public LabeledTextBoxContext condition { get; set; }
        public LabeledTextBoxContext fitness { get; set; }
        public LabeledTextBoxContext morale { get; set; }
        public LabeledTextBoxContext jadedness { get; set; }
        public LabeledTextBoxContext happiness { get; set; }
        public LabeledTextBoxContext squadno { get; set; }
        public LabeledTextBoxContext leftfoot { get; set; }
        public LabeledTextBoxContext rightfoot { get; set; }

        // attributes
        public LabeledTextBoxContext corners { get; set; }
        public LabeledTextBoxContext crossing { get; set; }
        public LabeledTextBoxContext dribbling { get; set; }
        public LabeledTextBoxContext finishing { get; set; }
        public LabeledTextBoxContext firsttouch { get; set; }
        public LabeledTextBoxContext freekicks { get; set; }
        public LabeledTextBoxContext heading { get; set; }
        public LabeledTextBoxContext longshots { get; set; }
        public LabeledTextBoxContext longthrows { get; set; }
        public LabeledTextBoxContext marking { get; set; }
        public LabeledTextBoxContext passing { get; set; }
        public LabeledTextBoxContext penaltytaking { get; set; }
        public LabeledTextBoxContext tackling { get; set; }
        public LabeledTextBoxContext technique { get; set; }
        public LabeledTextBoxContext consistency { get; set; }
        public LabeledTextBoxContext dirtyness { get; set; }
        public LabeledTextBoxContext importantmatches { get; set; }
        public LabeledTextBoxContext injuryproneness { get; set; }
        public LabeledTextBoxContext versatility { get; set; }
        public LabeledTextBoxContext acceleration { get; set; }
        public LabeledTextBoxContext agility { get; set; }
        public LabeledTextBoxContext balance { get; set; }
        public LabeledTextBoxContext jumping { get; set; }
        public LabeledTextBoxContext naturalfitness { get; set; }
        public LabeledTextBoxContext pace { get; set; }
        public LabeledTextBoxContext stamina { get; set; }
        public LabeledTextBoxContext strength { get; set; }
        public LabeledTextBoxContext aerialability { get; set; }
        public LabeledTextBoxContext commandofarea { get; set; }
        public LabeledTextBoxContext communication { get; set; }
        public LabeledTextBoxContext eccentricity { get; set; }
        public LabeledTextBoxContext handling { get; set; }
        public LabeledTextBoxContext kicking { get; set; }
        public LabeledTextBoxContext oneonones { get; set; }
        public LabeledTextBoxContext reflexes { get; set; }
        public LabeledTextBoxContext rushingout { get; set; }
        public LabeledTextBoxContext tendencytopunch { get; set; }
        public LabeledTextBoxContext throwing { get; set; }
        public LabeledTextBoxContext aggression { get; set; }
        public LabeledTextBoxContext anticipation { get; set; }
        public LabeledTextBoxContext bravery { get; set; }
        public LabeledTextBoxContext composure { get; set; }
        public LabeledTextBoxContext concentration { get; set; }
        public LabeledTextBoxContext creativity { get; set; }
        public LabeledTextBoxContext decisions { get; set; }
        public LabeledTextBoxContext determination { get; set; }
        public LabeledTextBoxContext flair { get; set; }
        public LabeledTextBoxContext influence { get; set; }
        public LabeledTextBoxContext offtheball { get; set; }
        public LabeledTextBoxContext positioning { get; set; }
        public LabeledTextBoxContext teamwork { get; set; }
        public LabeledTextBoxContext workrate { get; set; }
        public LabeledTextBoxContext adaptability { get; set; }
        public LabeledTextBoxContext ambition { get; set; }
        public LabeledTextBoxContext controversy { get; set; }
        public LabeledTextBoxContext loyalty { get; set; }
        public LabeledTextBoxContext pressure { get; set; }
        public LabeledTextBoxContext professionalism { get; set; }
        public LabeledTextBoxContext sportsmanship { get; set; }
        public LabeledTextBoxContext temperament { get; set; }
        public LabeledHeaderContext bestpr { get; set; }
        public PlayerRatingsContext GK { get; set; }
        public PlayerRatingsContext SW { get; set; }
        public PlayerRatingsContext DL { get; set; }
        public PlayerRatingsContext DC { get; set; }
        public PlayerRatingsContext DR { get; set; }
        public PlayerRatingsContext DML { get; set; }
        public PlayerRatingsContext DMC { get; set; }
        public PlayerRatingsContext DMR { get; set; }
        public PlayerRatingsContext AML { get; set; }
        public PlayerRatingsContext AMC { get; set; }
        public PlayerRatingsContext AMR { get; set; }
        public PlayerRatingsContext FCFast { get; set; }
        public PlayerRatingsContext FCTarget { get; set; }

        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private List<LabeledTextBoxContext> contexts = null;
		private List<PlayerRatingsContext> contextsratings = null;

        public void setProfileViewModel(ref Player player, ref PlayerGridViewModel r)
        {
            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            this.SelectionButton = new CheckBoxContext();
            this.SelectionButton.IsChecked = true;

            int index = ScoutLocalization.SR_PLAYERCOLUMNSCORNER;

            contexts = new List<LabeledTextBoxContext>();
            for (int i = index; i < globalFuncs.localization.playerColumns.Count; ++i)
            {
                LabeledTextBoxContext _context = new LabeledTextBoxContext();
                setAttributeContext(ref _context);
                contexts.Add(_context);
            }

            contextsratings = new List<PlayerRatingsContext>();
			for (int i = 1; i < globalFuncs.localization.bestprs.Count; ++i)
            {
                PlayerRatingsContext _context = new PlayerRatingsContext();
				contextsratings.Add(_context);
			}

            this.shortlisttext = new LabeledHeaderContext();
            this.viewtechnical = new LabeledHeaderContext();
            this.viewgoalkeeping = new LabeledHeaderContext();
            this.position = new LabeledHeaderContext();
            this.name = new LabeledHeaderContext();
            this.fullname = new LabeledHeaderContext();
            this.nation = new LabeledHeaderContext();
            this.formed = new LabeledHeaderContext();
            this.eu = new LabeledHeaderContext();
            this.countryhg = new LabeledHeaderContext();
            this.birthdate = new LabeledHeaderContext();
            this.age = new LabeledHeaderContext();
            this.height = new LabeledHeaderContext();
            this.weight = new LabeledHeaderContext();
            this.value = new LabeledHeaderContext();
            this.salevalue = new LabeledHeaderContext();
            this.international = new LabeledHeaderContext();
            this.preffoot = new LabeledHeaderContext();
            this.ca = new LabeledHeaderContext();
            this.pa = new LabeledHeaderContext();
            this.regen = new LabeledHeaderContext();
            this.club = new LabeledHeaderContext();
            this.curclub = new LabeledHeaderContext();

            teamsquad = new LabeledTextBoxContext();
            contractstarted = new LabeledTextBoxContext();
            contractexpiring = new LabeledTextBoxContext();
            wage = new LabeledTextBoxContext();
            appbonus = new LabeledTextBoxContext();
            goalbonus = new LabeledTextBoxContext();
            cleanbonus = new LabeledTextBoxContext();
            
            condition = new LabeledTextBoxContext();
            fitness = new LabeledTextBoxContext();
            morale = new LabeledTextBoxContext();
            jadedness = new LabeledTextBoxContext();
            happiness = new LabeledTextBoxContext();
            squadno = new LabeledTextBoxContext();
            leftfoot = new LabeledTextBoxContext();
            rightfoot = new LabeledTextBoxContext();

            this.technicalattributes = new LabeledHeaderContext();
            this.physicalattributes = new LabeledHeaderContext();
            this.mentalattributes = new LabeledHeaderContext();
            this.hiddenattributes = new LabeledHeaderContext();
            this.goalkeepingattributes = new LabeledHeaderContext();
            this.mentaltraitsattributes = new LabeledHeaderContext();
            this.personaldetails = new LabeledHeaderContext();
            this.contractdetails = new LabeledHeaderContext();
            this.otherattributes = new LabeledHeaderContext();
            this.bestpr = new LabeledHeaderContext();

            setProfileContext(teamsquad);
            setProfileContext(contractstarted);
            setProfileContext(contractexpiring);
            setProfileContext(wage);
            setProfileContext(appbonus);
            setProfileContext(goalbonus);
            setProfileContext(cleanbonus);

            setAttributeContext(condition);
            setAttributeContext(fitness);
            setAttributeContext(morale);
            setAttributeContext(jadedness);
            setAttributeContext(happiness);
            setAttributeContext(squadno);
            setAttributeContext(leftfoot);
            setAttributeContext(rightfoot);

            setControlValues(ref player, ref r);
            setLocalization();
        }

        public void setControlValues(ref Player player, ref PlayerGridViewModel r)
        {
            PreferencesSettings curSettings = settings.curPreferencesSettings;
            ObservableCollection<String> ProfileGenericLabels = globalFuncs.localization.ProfileGenericLabels;

            String wageExtended = globalFuncs.localization.wages
                [globalFuncs.localization.wagesNative.IndexOf(curSettings.wageMultiplier.extended)];

            String heightExtended = "";
            if (curSettings.heightMultiplier.extended.Equals("cm"))
                heightExtended = globalFuncs.localization.WindowGeneralLabels[ScoutLocalization.WG_CM];
            else if (curSettings.heightMultiplier.extended.Equals("m"))
                heightExtended = globalFuncs.localization.WindowGeneralLabels[ScoutLocalization.WG_M];

            String weightExtended = "";
            if (curSettings.weightMultiplier.extended.Equals("kg"))
                weightExtended = globalFuncs.localization.WindowGeneralLabels[ScoutLocalization.WG_KG];
            else if (curSettings.weightMultiplier.extended.Equals("lbs"))
                weightExtended = globalFuncs.localization.WindowGeneralLabels[ScoutLocalization.WG_LBS];

            this.shortlistchecked = r.S;
            if (r.S)
                this.shortlisttext.Header = ProfileGenericLabels[ScoutLocalization.PG_REMOVEFROMSHORTLIST];
            else
                this.shortlisttext.Header = ProfileGenericLabels[ScoutLocalization.PG_ADDTOSHORTLIST];

            this.ID = r.ID;
            if (player.Nickname.Length > 0)
                this.name.Header += player.Nickname;
            else
                this.name.Header = player.ToString();

            this.SelectionButton.Content = (String)this.name.Header;

            this.fullname.Header = player.ToString();
            if (player.Nickname.Length > 0)
                this.fullname.Header += " (" + player.Nickname + ")";

            this.curclub.Header = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER];
            this.club.Header = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER];
            string playerContractExpiryDate = ProfileGenericLabels[ScoutLocalization.PG_NOCONTRACT];
            string playerContractStartDate = ProfileGenericLabels[ScoutLocalization.PG_NOCONTRACT];
            string teamSquad = ProfileGenericLabels[ScoutLocalization.PG_NONE];
            int playerContractWage = 0;
            int playerAppearance = 0;
            int playerGoal = 0;
            int playerCleanSheet = 0;
            Contract contract = null;
            if (player.Contract != null)
            {
                this.curclub.Header = player.Contract.Club.Name;
                this.club.Header = player.Contract.Club.Name;
                teamSquad = player.Team.Type.ToString();
                playerContractExpiryDate = player.Contract.ContractExpiryDate.ToShortDateString();
                playerContractStartDate = player.Contract.ContractStarted.ToShortDateString();
                playerContractWage = player.Contract.WagePerWeek;
                playerAppearance = player.Contract.AppearanceBonus;
                playerGoal = player.Contract.GoalBonus;
                playerCleanSheet = player.Contract.CleanSheetBonus;
                contract = player.Contract;
                if (player.ContractSecond != null)
                {
                    if (player.ContractSecond.ContractTypeSecond == ContractTypeSecond.Coowned &&
                        player.ContractSecond.Club != null)
                    {
                        this.curclub.Header = player.Contract.Club.Name;
                        this.club.Header = player.Contract.Club.Name + "/" + player.ContractSecond.Club.Name;
                    }
                    else if (player.ContractSecond.ContractTypeSecond == ContractTypeSecond.Loan &&
                        player.ContractSecond.Club != null)
                    {
                        this.curclub.Header = player.ContractSecond.Club.Name;
                        this.club.Header = player.ContractSecond.Club.Name + " (" + player.Contract.Club.Name + ")";
                        playerContractExpiryDate = player.ContractSecond.ContractExpiryDate.ToShortDateString();
                        playerContractStartDate = player.ContractSecond.ContractStarted.ToShortDateString();
                        playerContractWage = player.ContractSecond.WagePerWeek;
                        playerAppearance = 0;
                        playerGoal = 0;
                        playerCleanSheet = 0;
                        contract = player.ContractSecond;
                    }
                }
            }

            this.teamsquad.TextBoxText = teamSquad;

            if (player.ID < 1394640000) this.regen.Header = ProfileGenericLabels[ScoutLocalization.PG_NOTAREGEN];
            else this.regen.Header = ProfileGenericLabels[ScoutLocalization.PG_ISREGEN];

            bool EUmember = false;
            this.nation.Header = ProfileGenericLabels[ScoutLocalization.PG_UNKNOWN];

            if (player.Nationality != null)
            {
                this.nation.Header = player.Nationality.Name + " (" + player.Nationality.Continent.Name + ")";
                if (globalFuncs.EUcountries.Contains(player.Nationality.Name)) EUmember = true;
            }

            // other nationalities
            if (player.Relations != null)
            {
                List<PlayerRelations> relations = player.Relations.Relations;
                for (int playerRelationIndex = 0; playerRelationIndex < player.Relations.RelationsTotal; ++playerRelationIndex)
                {
                    if (relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                    {
                        if (!EUmember)
                        {
                            if (globalFuncs.EUcountries.Contains(relations[playerRelationIndex].Country.Name))
                                EUmember = true;
                        }
                        this.nation.Header += "," + Environment.NewLine + relations[playerRelationIndex].Country.Name + " (" + relations[playerRelationIndex].Country.Continent.Name + ")";
                    }
                }

                // formed
                bool formedDone = false;
                for (int playerRelationIndex = 0; playerRelationIndex < player.Relations.RelationsTotal; ++playerRelationIndex)
                {
                    if (relations[playerRelationIndex].RelationType == RelationType.FormedAtClub)
                    {
                        if (contract != null)
                        {
                            if (relations[playerRelationIndex].Team.Club.ID.Equals(contract.Club.ID))
                                this.countryhg.Header = ProfileGenericLabels[ScoutLocalization.PG_CLUBHG];
                        }

                        for (int playerRelationIndex2 = 0; playerRelationIndex2 < player.Relations.RelationsTotal; ++playerRelationIndex2)
                        {
                            if (relations[playerRelationIndex2].RelationType == RelationType.FormedAtCountry)
                            {
                                this.formed.Header = ProfileGenericLabels[ScoutLocalization.PG_FORMEDAT] + " " +
                                    relations[playerRelationIndex].Team.Club.Name + " (" + relations[playerRelationIndex2].Country.Name + ")";
                                if (contract != null)
                                {
                                    if (this.countryhg.Header.ToString().Length == 0)
                                    {
                                        if (!countryhg.Header.Equals(ProfileGenericLabels[ScoutLocalization.PG_CLUBHG]) &&
                                        relations[playerRelationIndex2].Country.Name.Equals(contract.Club.Country.Name))
                                            this.countryhg.Header = ProfileGenericLabels[ScoutLocalization.PG_COUNTRYHG];
                                    }
                                    formedDone = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (formedDone) break;
                }
            }

            if (this.countryhg.Header.ToString().Length == 0)
                this.countryhg.Header = ProfileGenericLabels[ScoutLocalization.PG_NOTHG];

            if (this.formed.Header.ToString().Length == 0)
                this.formed.Header = ProfileGenericLabels[ScoutLocalization.PG_FORMEDAT] + " " + ProfileGenericLabels[ScoutLocalization.PG_UNKNOWN];

            this.eu.Header = ProfileGenericLabels[ScoutLocalization.PG_EUMEMBER] + " ";
            if (EUmember) this.eu.Header += ProfileGenericLabels[ScoutLocalization.PG_YES];
            else this.eu.Header += ProfileGenericLabels[ScoutLocalization.PG_NO];

            if (player.InternationalCaps != 0)
                this.international.Header = player.InternationalCaps.ToString() + " " + 
                    ProfileGenericLabels[ScoutLocalization.PG_CAPS] +
                    "/" + player.InternationalGoals.ToString() + " " +
                    ProfileGenericLabels[ScoutLocalization.PG_GOALS];
            else
                this.international.Header = ProfileGenericLabels[ScoutLocalization.PG_UNCAPPED];
            this.birthdate.Header = player.DateOfBirth.Date.ToShortDateString();
            this.age.Header = player.Age.ToString() + " " + ProfileGenericLabels[ScoutLocalization.PG_YEARSOLD];
            this.height.Header = (float.Parse(player.Height.ToString()) * curSettings.heightMultiplier.multiplier).ToString() + " " + heightExtended;
            this.weight.Header = (float.Parse(player.Weight.ToString()) * curSettings.weightMultiplier.multiplier).ToString() + " " + weightExtended;
            
            this.contractstarted.TextBoxText = playerContractStartDate;
            this.contractexpiring.TextBoxText = playerContractExpiryDate;

            this.value.Header = (player.Value * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);
            if (player.SaleValue != -1)
                this.salevalue.Header = (player.SaleValue * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);
            else
                this.salevalue.Header = ProfileGenericLabels[ScoutLocalization.PG_NOSALEVALUE];

            if (player.Skills.LeftFoot > 75 && player.Skills.RightFoot > 75)
                this.preffoot.Header = ProfileGenericLabels[ScoutLocalization.PG_BOTHFEET];
            else if (player.Skills.LeftFoot > 75)
                this.preffoot.Header = ProfileGenericLabels[ScoutLocalization.PG_LEFTFOOT];
            else if (player.Skills.RightFoot > 75)
                this.preffoot.Header = ProfileGenericLabels[ScoutLocalization.PG_RIGHTFOOT];
            this.localrep = player.CurrentReputation;
            this.nationalrep = player.HomeReputation;
            this.worldrep = player.WorldReputation;
            this.ca.Header = player.CurrentPlayingAbility.ToString() + " " + globalFuncs.localization.PlayerSearchLabels[ScoutLocalization.L_CA];
            this.pa.Header = player.PotentialPlayingAbility.ToString() + " " + globalFuncs.localization.PlayerSearchLabels[ScoutLocalization.L_PA];
            string playerPosition = "";
            List<string> positions = new List<string>();
            List<string> sides = new List<string>();
            context.find_player_position(player, ref playerPosition, ref positions, ref sides, true);

            this.position.Header = playerPosition;

            this.appbonus.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBONUS];
            this.wage.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOWAGE];
            this.goalbonus.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBONUS];
            this.cleanbonus.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOBONUS];
            if (playerAppearance != 0)
                this.appbonus.TextBoxText = (playerAppearance * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);

            if (playerContractWage != 0)
                this.wage.TextBoxText = (playerContractWage * curSettings.wageMultiplier.multiplier * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format) + " " + wageExtended;

            if (playerGoal != 0)
                this.goalbonus.TextBoxText = (playerGoal * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);

            if (playerCleanSheet != 0)
                this.cleanbonus.TextBoxText = (playerCleanSheet * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format);

            // other
            this.condition.TextBoxText = (((int)player.Condition * 0.0001f)).ToString("P0", curSettings.currencyMultiplier.format);
            this.fitness.TextBoxText = (((int)player.Fitness * 0.0001f)).ToString("P0", curSettings.currencyMultiplier.format);
            this.morale.TextBoxText = player.Morale.ToString();
            if (player.Contract != null)
                this.happiness.TextBoxText = (player.Contract.Happiness * 0.01f).ToString("P0", curSettings.currencyMultiplier.format);
            this.jadedness.TextBoxText = player.Jadeness;
            this.squadno.TextBoxText = "?";
            //this.squadno.TextBoxText = player.Contract.SquadNumber.ToString();
            this.leftfoot.TextBoxText = ((int)(player.Skills.LeftFoot * 0.2 + 0.5)).ToString();
            this.rightfoot.TextBoxText = ((int)(player.Skills.RightFoot * 0.2 + 0.5)).ToString();

            this.worldrep = r.WorldReputation;
            this.nationalrep = r.NationalReputation;
            this.localrep = r.LocalReputation;

			// positional ratings
            if (!context.playersPRID.Contains(r.ID))
                context.calculatePlayerPR(player);
            PositionalRatings pr = (PositionalRatings)context.playersPRID[r.ID];
            int index = -1;
            this.GK = contextsratings[++index];
            this.GK.LabelContent = (pr.GK * 0.01f).ToString("P2");
            this.DC = contextsratings[++index];
            this.DC.LabelContent = (pr.DC * 0.01f).ToString("P2");
            this.DL = contextsratings[++index];
            this.DL.LabelContent = (pr.DL * 0.01f).ToString("P2");
            this.DR = contextsratings[++index];
            this.DR.LabelContent = (pr.DR * 0.01f).ToString("P2");
            this.DMC = contextsratings[++index];
            this.DMC.LabelContent = (pr.DMC * 0.01f).ToString("P2");
            this.DML = contextsratings[++index];
            this.DML.LabelContent = (pr.DML * 0.01f).ToString("P2");
            this.DMR = contextsratings[++index];
            this.DMR.LabelContent = (pr.DMR * 0.01f).ToString("P2");
            this.AMC = contextsratings[++index];
            this.AMC.LabelContent = (pr.AMC * 0.01f).ToString("P2");
            this.AML = contextsratings[++index];
            this.AML.LabelContent = (pr.AML * 0.01f).ToString("P2");
            this.AMR = contextsratings[++index];
            this.AMR.LabelContent = (pr.AMR * 0.01f).ToString("P2");
            this.FCFast = contextsratings[++index];
            this.FCFast.LabelContent = (pr.FCFast * 0.01f).ToString("P2");
            this.FCTarget = contextsratings[++index];
            this.FCTarget.LabelContent = (pr.FCTarget * 0.01f).ToString("P2");

            if (pr.GK == pr.bestPosR) this.GK.LabelFontWeight = FontWeights.Bold;
            if (pr.DC == pr.bestPosR) this.DC.LabelFontWeight = FontWeights.Bold;
            if (pr.DL == pr.bestPosR) this.DL.LabelFontWeight = FontWeights.Bold;
            if (pr.DR == pr.bestPosR) this.DR.LabelFontWeight = FontWeights.Bold;
            if (pr.DMC == pr.bestPosR) this.DMC.LabelFontWeight = FontWeights.Bold;
            if (pr.DML == pr.bestPosR) this.DML.LabelFontWeight = FontWeights.Bold;
            if (pr.DMR == pr.bestPosR) this.DMR.LabelFontWeight = FontWeights.Bold;
            if (pr.AMC == pr.bestPosR) this.AMC.LabelFontWeight = FontWeights.Bold;
            if (pr.AML == pr.bestPosR) this.AML.LabelFontWeight = FontWeights.Bold;
            if (pr.AMR == pr.bestPosR) this.AMR.LabelFontWeight = FontWeights.Bold;
            if (pr.FCFast == pr.bestPosR) this.FCFast.LabelFontWeight = FontWeights.Bold;
            if (pr.FCTarget == pr.bestPosR) this.FCTarget.LabelFontWeight = FontWeights.Bold;
					
            index = -1;
            this.corners = contexts[++index];
            this.crossing = contexts[++index];
            this.dribbling = contexts[++index];
            this.finishing = contexts[++index];
            this.firsttouch = contexts[++index];
            this.freekicks = contexts[++index];
            this.heading = contexts[++index];
            this.longshots = contexts[++index];
            this.longthrows = contexts[++index];
            this.marking = contexts[++index];
            this.passing = contexts[++index];
            this.penaltytaking = contexts[++index];
            this.tackling = contexts[++index];
            this.technique = contexts[++index];
            this.acceleration = contexts[++index];
            this.agility = contexts[++index];
            this.balance = contexts[++index];
            this.jumping = contexts[++index];
            this.naturalfitness = contexts[++index];
            this.pace = contexts[++index];
            this.stamina = contexts[++index];
            this.strength = contexts[++index];
            this.leftfoot = contexts[++index];
            this.rightfoot = contexts[++index];
            this.aggression = contexts[++index];
            this.anticipation = contexts[++index];
            this.bravery = contexts[++index];
            this.composure = contexts[++index];
            this.concentration = contexts[++index];
            this.creativity = contexts[++index];
            this.decisions = contexts[++index];
            this.determination = contexts[++index];
            this.flair = contexts[++index];
            this.influence = contexts[++index];
            this.offtheball = contexts[++index];
            this.positioning = contexts[++index];
            this.teamwork = contexts[++index];
            this.workrate = contexts[++index];
            this.consistency = contexts[++index];
            this.dirtyness = contexts[++index];
            this.importantmatches = contexts[++index];
            this.injuryproneness = contexts[++index];
            this.versatility = contexts[++index];
            this.aerialability = contexts[++index];
            this.commandofarea = contexts[++index];
            this.communication = contexts[++index];
            this.eccentricity = contexts[++index];
            this.handling = contexts[++index];
            this.kicking = contexts[++index];
            this.oneonones = contexts[++index];
            this.reflexes = contexts[++index];
            this.rushingout = contexts[++index];
            this.tendencytopunch = contexts[++index];
            this.throwing = contexts[++index];
            this.adaptability = contexts[++index];
            this.ambition = contexts[++index];
            this.controversy = contexts[++index];
            this.loyalty = contexts[++index];
            this.pressure = contexts[++index];
            this.professionalism = contexts[++index];
            this.sportsmanship = contexts[++index];
            this.temperament = contexts[++index];

            this.corners.TextBoxText = r.Corners;
            this.crossing.TextBoxText = r.Crossing;
            this.dribbling.TextBoxText = r.Dribbling;
            this.finishing.TextBoxText = r.Finishing;
            this.firsttouch.TextBoxText = r.FirstTouch;
            this.freekicks.TextBoxText = r.FreeKicks;
            this.heading.TextBoxText = r.Heading;
            this.longshots.TextBoxText = r.LongShots;
            this.longthrows.TextBoxText = r.LongThrows;
            this.marking.TextBoxText = r.Marking;
            this.passing.TextBoxText = r.Passing;
            this.penaltytaking.TextBoxText = r.PenaltyTaking;
            this.tackling.TextBoxText = r.Tackling;
            this.technique.TextBoxText = r.Technique;
            this.acceleration.TextBoxText = r.Acceleration;
            this.agility.TextBoxText = r.Agility;
            this.balance.TextBoxText = r.Balance;
            this.jumping.TextBoxText = r.Jumping;
            this.naturalfitness.TextBoxText = r.NaturalFitness;
            this.pace.TextBoxText = r.Pace;
            this.stamina.TextBoxText = r.Stamina;
            this.strength.TextBoxText = r.Strength;
            this.leftfoot.TextBoxText = r.LeftFoot;
            this.rightfoot.TextBoxText = r.RightFoot;
            this.aggression.TextBoxText = r.Aggression;
            this.anticipation.TextBoxText = r.Anticipation;
            this.bravery.TextBoxText = r.Bravery;
            this.composure.TextBoxText = r.Composure;
            this.concentration.TextBoxText = r.Concentration;
            this.creativity.TextBoxText = r.Creativity;
            this.decisions.TextBoxText = r.Decisions;
            this.determination.TextBoxText = r.Determination;
            this.flair.TextBoxText = r.Flair;
            this.influence.TextBoxText = r.Influence;
            this.offtheball.TextBoxText = r.OffTheBall;
            this.positioning.TextBoxText = r.Positioning;
            this.teamwork.TextBoxText = r.TeamWork;
            this.workrate.TextBoxText = r.WorkRate;
            this.consistency.TextBoxText = r.Consistency;
            this.dirtyness.TextBoxText = r.Dirtyness;
            this.importantmatches.TextBoxText = r.ImportantMatches;
            this.injuryproneness.TextBoxText = r.InjuryProneness;
            this.versatility.TextBoxText = r.Versatility;
            this.aerialability.TextBoxText = r.AerialAbility;
            this.commandofarea.TextBoxText = r.CommandOfArea;
            this.communication.TextBoxText = r.Communication;
            this.eccentricity.TextBoxText = r.Eccentricity;
            this.handling.TextBoxText = r.Handling;
            this.kicking.TextBoxText = r.Kicking;
            this.oneonones.TextBoxText = r.OneOnOnes;
            this.reflexes.TextBoxText = r.Reflexes;
            this.rushingout.TextBoxText = r.RushingOut;
            this.tendencytopunch.TextBoxText = r.TendencyToPunch;
            this.throwing.TextBoxText = r.Throwing;
            this.adaptability.TextBoxText = r.Adaptability;
            this.ambition.TextBoxText = r.Ambition;
            this.controversy.TextBoxText = r.Controversy;
            this.loyalty.TextBoxText = r.Loyalty;
            this.pressure.TextBoxText = r.Pressure;
            this.professionalism.TextBoxText = r.Professionalism;
            this.sportsmanship.TextBoxText = r.Sportsmanship;
            this.temperament.TextBoxText = r.Temperament;

            this.corners.TextBoxForeground = globalFuncs.setAttributeForeground(r.Corners);
            this.crossing.TextBoxForeground = globalFuncs.setAttributeForeground(r.Crossing);
            this.dribbling.TextBoxForeground = globalFuncs.setAttributeForeground(r.Dribbling);
            this.finishing.TextBoxForeground = globalFuncs.setAttributeForeground(r.Finishing);
            this.firsttouch.TextBoxForeground = globalFuncs.setAttributeForeground(r.FirstTouch);
            this.freekicks.TextBoxForeground = globalFuncs.setAttributeForeground(r.FreeKicks);
            this.heading.TextBoxForeground = globalFuncs.setAttributeForeground(r.Heading);
            this.longshots.TextBoxForeground = globalFuncs.setAttributeForeground(r.LongShots);
            this.longthrows.TextBoxForeground = globalFuncs.setAttributeForeground(r.LongThrows);
            this.marking.TextBoxForeground = globalFuncs.setAttributeForeground(r.Marking);
            this.passing.TextBoxForeground = globalFuncs.setAttributeForeground(r.Passing);
            this.penaltytaking.TextBoxForeground = globalFuncs.setAttributeForeground(r.PenaltyTaking);
            this.tackling.TextBoxForeground = globalFuncs.setAttributeForeground(r.Tackling);
            this.technique.TextBoxForeground = globalFuncs.setAttributeForeground(r.Technique);
            this.acceleration.TextBoxForeground = globalFuncs.setAttributeForeground(r.Acceleration);
            this.agility.TextBoxForeground = globalFuncs.setAttributeForeground(r.Agility);
            this.balance.TextBoxForeground = globalFuncs.setAttributeForeground(r.Balance);
            this.jumping.TextBoxForeground = globalFuncs.setAttributeForeground(r.Jumping);
            this.naturalfitness.TextBoxForeground = globalFuncs.setAttributeForeground(r.NaturalFitness);
            this.pace.TextBoxForeground = globalFuncs.setAttributeForeground(r.Pace);
            this.stamina.TextBoxForeground = globalFuncs.setAttributeForeground(r.Stamina);
            this.strength.TextBoxForeground = globalFuncs.setAttributeForeground(r.Strength);
            this.leftfoot.TextBoxForeground = globalFuncs.setAttributeForeground(r.LeftFoot);
            this.rightfoot.TextBoxForeground = globalFuncs.setAttributeForeground(r.RightFoot);
            this.aggression.TextBoxForeground = globalFuncs.setAttributeForeground(r.Aggression);
            this.anticipation.TextBoxForeground = globalFuncs.setAttributeForeground(r.Anticipation);
            this.bravery.TextBoxForeground = globalFuncs.setAttributeForeground(r.Bravery);
            this.composure.TextBoxForeground = globalFuncs.setAttributeForeground(r.Composure);
            this.concentration.TextBoxForeground = globalFuncs.setAttributeForeground(r.Concentration);
            this.creativity.TextBoxForeground = globalFuncs.setAttributeForeground(r.Creativity);
            this.decisions.TextBoxForeground = globalFuncs.setAttributeForeground(r.Decisions);
            this.determination.TextBoxForeground = globalFuncs.setAttributeForeground(r.Determination);
            this.flair.TextBoxForeground = globalFuncs.setAttributeForeground(r.Flair);
            this.influence.TextBoxForeground = globalFuncs.setAttributeForeground(r.Influence);
            this.offtheball.TextBoxForeground = globalFuncs.setAttributeForeground(r.OffTheBall);
            this.positioning.TextBoxForeground = globalFuncs.setAttributeForeground(r.Positioning);
            this.teamwork.TextBoxForeground = globalFuncs.setAttributeForeground(r.TeamWork);
            this.workrate.TextBoxForeground = globalFuncs.setAttributeForeground(r.WorkRate);
            this.consistency.TextBoxForeground = globalFuncs.setAttributeForeground(r.Consistency);
            this.dirtyness.TextBoxForeground = globalFuncs.setAttributeForeground(r.Dirtyness);
            this.importantmatches.TextBoxForeground = globalFuncs.setAttributeForeground(r.ImportantMatches);
            this.injuryproneness.TextBoxForeground = globalFuncs.setAttributeForeground(r.InjuryProneness);
            this.versatility.TextBoxForeground = globalFuncs.setAttributeForeground(r.Versatility);
            this.aerialability.TextBoxForeground = globalFuncs.setAttributeForeground(r.AerialAbility);
            this.commandofarea.TextBoxForeground = globalFuncs.setAttributeForeground(r.CommandOfArea);
            this.communication.TextBoxForeground = globalFuncs.setAttributeForeground(r.Communication);
            this.eccentricity.TextBoxForeground = globalFuncs.setAttributeForeground(r.Eccentricity);
            this.handling.TextBoxForeground = globalFuncs.setAttributeForeground(r.Handling);
            this.kicking.TextBoxForeground = globalFuncs.setAttributeForeground(r.Kicking);
            this.oneonones.TextBoxForeground = globalFuncs.setAttributeForeground(r.OneOnOnes);
            this.reflexes.TextBoxForeground = globalFuncs.setAttributeForeground(r.Reflexes);
            this.rushingout.TextBoxForeground = globalFuncs.setAttributeForeground(r.RushingOut);
            this.tendencytopunch.TextBoxForeground = globalFuncs.setAttributeForeground(r.TendencyToPunch);
            this.throwing.TextBoxForeground = globalFuncs.setAttributeForeground(r.Throwing);
            this.adaptability.TextBoxForeground = globalFuncs.setAttributeForeground(r.Adaptability);
            this.ambition.TextBoxForeground = globalFuncs.setAttributeForeground(r.Ambition);
            this.controversy.TextBoxForeground = globalFuncs.setAttributeForeground(r.Controversy);
            this.loyalty.TextBoxForeground = globalFuncs.setAttributeForeground(r.Loyalty);
            this.pressure.TextBoxForeground = globalFuncs.setAttributeForeground(r.Pressure);
            this.professionalism.TextBoxForeground = globalFuncs.setAttributeForeground(r.Professionalism);
            this.sportsmanship.TextBoxForeground = globalFuncs.setAttributeForeground(r.Sportsmanship);
            this.temperament.TextBoxForeground = globalFuncs.setAttributeForeground(r.Temperament);

            this.GK.LabelForeground = globalFuncs.setRatingForeground(pr.GK);
            this.DC .LabelForeground = globalFuncs.setRatingForeground(pr.DC);
            this.DL .LabelForeground = globalFuncs.setRatingForeground(pr.DL);
            this.DR.LabelForeground = globalFuncs.setRatingForeground(pr.DR);
            this.DMC.LabelForeground = globalFuncs.setRatingForeground(pr.DMC);
            this.DML.LabelForeground = globalFuncs.setRatingForeground(pr.DML);
            this.DMR.LabelForeground = globalFuncs.setRatingForeground(pr.DMR);
            this.AMC.LabelForeground = globalFuncs.setRatingForeground(pr.AMC);
            this.AML.LabelForeground = globalFuncs.setRatingForeground(pr.AML);
            this.AMR.LabelForeground = globalFuncs.setRatingForeground(pr.AMR);
            this.FCFast.LabelForeground = globalFuncs.setRatingForeground(pr.FCFast);
            this.FCTarget.LabelForeground = globalFuncs.setRatingForeground(pr.FCTarget);
        }        
                 
        public void setLocalization()
        {        
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> playerSearchAttributesGroupBoxes = localization.playerSearchAttributesGroupBoxes;

            int index = -1;
            this.technicalattributes.Header = playerSearchAttributesGroupBoxes[++index];
            this.physicalattributes.Header = playerSearchAttributesGroupBoxes[++index];
            this.mentalattributes.Header = playerSearchAttributesGroupBoxes[++index];
            this.hiddenattributes.Header = playerSearchAttributesGroupBoxes[++index];
            this.goalkeepingattributes.Header = playerSearchAttributesGroupBoxes[++index];
            this.mentaltraitsattributes.Header = playerSearchAttributesGroupBoxes[++index];
            this.personaldetails.Header = playerSearchAttributesGroupBoxes[++index];
            this.contractdetails.Header = playerSearchAttributesGroupBoxes[++index];
            this.otherattributes.Header = playerSearchAttributesGroupBoxes[++index];

            index = ScoutLocalization.SR_PLAYERCOLUMNSCORNER;
            int counter = -1;
            for (int i = index; i < globalFuncs.localization.playerColumns.Count; ++i)
                contexts[++counter].LabelContent = globalFuncs.localization.playerColumns[i];

            ObservableCollection<String> PlayerProfileLabels = localization.PlayerProfileLabels;
            PositionalRatings pr = (PositionalRatings)context.playersPRID[this.ID];
            index = -1;
            this.viewtechnical.Header = PlayerProfileLabels[++index];
            this.viewgoalkeeping.Header = PlayerProfileLabels[++index];
            this.bestpr.Header = PlayerProfileLabels[++index] + Environment.NewLine + PlayerProfileLabels[++index] + pr.bestPos + Environment.NewLine + "(" + (pr.bestPosR * 0.01f).ToString("P2") + ")";
            this.teamsquad.LabelContent = PlayerProfileLabels[++index];
            this.contractstarted.LabelContent = PlayerProfileLabels[++index];
            this.contractexpiring.LabelContent = PlayerProfileLabels[++index];
            this.wage.LabelContent = PlayerProfileLabels[++index];
            this.appbonus.LabelContent = PlayerProfileLabels[++index];
            this.goalbonus.LabelContent = PlayerProfileLabels[++index];
            this.cleanbonus.LabelContent = PlayerProfileLabels[++index];
            this.condition.LabelContent = PlayerProfileLabels[++index];
            this.fitness.LabelContent = PlayerProfileLabels[++index];
            this.morale.LabelContent = PlayerProfileLabels[++index];
            this.jadedness.LabelContent = PlayerProfileLabels[++index];
            this.happiness.LabelContent = PlayerProfileLabels[++index];
            this.squadno.LabelContent = PlayerProfileLabels[++index];
            this.leftfoot.LabelContent = PlayerProfileLabels[++index];
            this.rightfoot.LabelContent = PlayerProfileLabels[++index];
        }
    }
}