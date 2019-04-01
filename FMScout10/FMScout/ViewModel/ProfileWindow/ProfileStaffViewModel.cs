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
    public class ProfileStaffViewModel : ProfileViewModel
    {
        // groupboxes
        public LabeledHeaderContext personaldetails { get; set; }
        public LabeledHeaderContext contractdetails { get; set; }
        public LabeledHeaderContext coachingattributes { get; set; }
        public LabeledHeaderContext chairmanattributes { get; set; }
        public LabeledHeaderContext mentalattributes { get; set; }
        public LabeledHeaderContext tacticalattributes { get; set; }
        public LabeledHeaderContext nontacticalattributes { get; set; }
        public LabeledHeaderContext ratings { get; set; }

        // header
        public String role { get; set; }

        // personal
        public String fullname { get; set; }
        public String nation { get; set; }
        public String birthdate { get; set; }
        public String age { get; set; }
        public String international { get; set; }
        public String ca { get; set; }
        public String pa { get; set; }
        public String regen { get; set; }

        // contract
        public String club { get; set; }
        public LabeledTextBoxContext contractstarted { get; set; }
        public LabeledTextBoxContext contractexpiring { get; set; }
        public LabeledTextBoxContext wage { get; set; }

        public int worldrep { get; set; }
        public int nationalrep { get; set; }
        public int localrep { get; set; }

        // attributes
        public LabeledTextBoxContext depth { get; set; }
        public LabeledTextBoxContext directness { get; set; }
        public LabeledTextBoxContext flamboyancy { get; set; }
        public LabeledTextBoxContext flexibility { get; set; }
        public LabeledTextBoxContext freeroles { get; set; }
        public LabeledTextBoxContext marking { get; set; }
        public LabeledTextBoxContext offside { get; set; }
        public LabeledTextBoxContext pressing { get; set; }
        public LabeledTextBoxContext sittingback { get; set; }
        public LabeledTextBoxContext tempo { get; set; }
        public LabeledTextBoxContext useofplaymaker { get; set; }
        public LabeledTextBoxContext useofsubstitutions { get; set; }
        public LabeledTextBoxContext width { get; set; }
        public LabeledTextBoxContext adaptability { get; set; }
        public LabeledTextBoxContext ambition { get; set; }
        public LabeledTextBoxContext controversy { get; set; }
        public LabeledTextBoxContext determination { get; set; }
        public LabeledTextBoxContext loyalty { get; set; }
        public LabeledTextBoxContext pressure { get; set; }
        public LabeledTextBoxContext professionalism { get; set; }
        public LabeledTextBoxContext sportsmanship { get; set; }
        //public LabeledTextBoxContext temperament { get; set; }
        public LabeledTextBoxContext judgingplayerability { get; set; }
        public LabeledTextBoxContext judgingplayerpotential { get; set; }
        public LabeledTextBoxContext levelofdiscipline { get; set; }
        public LabeledTextBoxContext motivating { get; set; }
        public LabeledTextBoxContext physiotherapy { get; set; }
        public LabeledTextBoxContext tacticalknowledge { get; set; }
        public LabeledTextBoxContext attacking { get; set; }
        public LabeledTextBoxContext defending { get; set; }
        public LabeledTextBoxContext fitness { get; set; }
        public LabeledTextBoxContext goalkeepers { get; set; }
        public LabeledTextBoxContext mental { get; set; }
        public LabeledTextBoxContext player { get; set; }
        public LabeledTextBoxContext tactical { get; set; }
        public LabeledTextBoxContext technical { get; set; }
        public LabeledTextBoxContext manmanagement { get; set; }
        public LabeledTextBoxContext workingwithyoungsters { get; set; }
        public LabeledTextBoxContext buyingplayers { get; set; }
        public LabeledTextBoxContext hardnessoftraining { get; set; }
        public LabeledTextBoxContext mindgames { get; set; }
        public LabeledTextBoxContext squadrotation { get; set; }
        public LabeledTextBoxContext business { get; set; }
        public LabeledTextBoxContext intereference { get; set; }
        public LabeledTextBoxContext patience { get; set; }
        public LabeledTextBoxContext resources { get; set; }
		
		public LabeledImageContext rfitness { get; set; }
		public LabeledImageContext rgoalkeepers { get; set; }
		public LabeledImageContext rtactics { get; set; }
		public LabeledImageContext rballcontrol { get; set; }
		public LabeledImageContext rdefending { get; set; }
		public LabeledImageContext rattacking { get; set; }
		public LabeledImageContext rshooting { get; set; }
		public LabeledImageContext rsetpieces { get; set; }

        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private List<LabeledTextBoxContext> contexts = null;
        private List<LabeledImageContext> contextsratings = null;

        protected override void setAttributeContext(ref LabeledTextBoxContext num)
        {
            num.LabelWidth = 130;
            num.LabelHeight = 19;
            num.TextBoxWidth = 38;
            num.TextBoxHeight = 19;
            num.TextBoxAlignment = HorizontalAlignment.Center;
        }

        public void setProfileViewModel(ref Staff staff, ref StaffGridViewModel r)
        {
            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            this.SelectionButton = new CheckBoxContext();
            this.SelectionButton.IsChecked = true;

            int index = ScoutLocalization.SR_STAFFCOLUMNDEPTH;

            contexts = new List<LabeledTextBoxContext>();
            for (int i = index; i < globalFuncs.localization.staffColumns.Count; ++i)
            {
                LabeledTextBoxContext _context = new LabeledTextBoxContext();
                setAttributeContext(ref _context);
                contexts.Add(_context);
            }

            contextsratings = new List<LabeledImageContext>();
            for (int i = 1; i < globalFuncs.localization.bestcrs.Count; ++i)
            {
				LabeledImageContext _context = new LabeledImageContext();
				contextsratings.Add(_context);
			}

            contractstarted = new LabeledTextBoxContext();
            contractexpiring = new LabeledTextBoxContext();
            wage = new LabeledTextBoxContext();

            this.tacticalattributes = new LabeledHeaderContext();
            this.mentalattributes = new LabeledHeaderContext();
            this.coachingattributes = new LabeledHeaderContext();
            this.nontacticalattributes = new LabeledHeaderContext();
            this.chairmanattributes = new LabeledHeaderContext();
            this.personaldetails = new LabeledHeaderContext();
            this.contractdetails = new LabeledHeaderContext();
            this.ratings = new LabeledHeaderContext();

            setProfileContext(contractstarted);
            setProfileContext(contractexpiring);
            setProfileContext(wage);

            setControlValues(ref staff, ref r);
            setLocalization();
        }

        public void setControlValues(ref Staff staff, ref StaffGridViewModel r)
        {
            PreferencesSettings curSettings = settings.curPreferencesSettings;
            ObservableCollection<String> ProfileGenericLabels = globalFuncs.localization.ProfileGenericLabels;

            String wageExtended = globalFuncs.localization.wages
                [globalFuncs.localization.wagesNative.IndexOf(curSettings.wageMultiplier.extended)];

            this.ID = r.ID;
            // personal details
            this.SelectionButton.Content = staff.ToString();
            this.fullname = this.SelectionButton.Content;
            this.club = r.Club;
            this.nation = staff.Nationality.Name + " (" + staff.Nationality.Continent.Name + ")";
            if (staff.InternationalCaps != 0)
                this.international = staff.InternationalCaps.ToString() + " " +
                    ProfileGenericLabels[ScoutLocalization.PG_CAPS] +
                    "/" + staff.InternationalGoals.ToString() + " " +
                    ProfileGenericLabels[ScoutLocalization.PG_GOALS];
            else
                this.international = ProfileGenericLabels[ScoutLocalization.PG_UNCAPPED];
            this.birthdate = staff.DateOfBirth.Date.ToShortDateString();
            this.age = staff.Age.ToString() + " " + ProfileGenericLabels[ScoutLocalization.PG_YEARSOLD];

            string staffRole = "";
            string empty = "";
            context.find_staff_role(staff, ref staffRole, ref empty, true);
            if (staffRole.Length == 0)
                this.role = ProfileGenericLabels[ScoutLocalization.PG_NOROLE];
            else
                this.role = staffRole;
            if (role.Equals(globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEAGENT]) || 
                role.Equals(globalFuncs.localization.SearchingResults[ScoutLocalization.R_CHAIRMAN])
                || role.Equals(globalFuncs.localization.SearchingResults[ScoutLocalization.R_DIRECTOR]) || staff.Contract == null)
            {
                this.contractstarted.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOCONTRACT];
                this.contractexpiring.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOCONTRACT];
                this.wage.TextBoxText = ProfileGenericLabels[ScoutLocalization.PG_NOWAGE];
            }
            else
            {
                this.contractstarted.TextBoxText = staff.Contract.ContractStarted.Date.ToShortDateString();
                this.contractexpiring.TextBoxText = staff.Contract.ContractExpiryDate.Date.ToShortDateString();
                this.wage.TextBoxText = (staff.Contract.WagePerWeek * curSettings.wageMultiplier.multiplier * curSettings.currencyMultiplier.multiplier).ToString("C0", curSettings.currencyMultiplier.format) + " " + wageExtended;
            }

            if (staff.NationalTeam != null)
            {
                if (staff.NationalTeam.Club != null)
                {
                    if (staff.NationalTeam.Club.Country != null)
                    {
                        if (globalFuncs.localization.regionsNative.Contains(staff.NationalTeam.Club.Country.Name))
                            this.club = staff.NationalTeam.Club.Name;
                    }
                }
            }

            this.ca = staff.CurrentCoachingAbility + " " + globalFuncs.localization.PlayerSearchLabels[ScoutLocalization.L_CA];
            this.pa = staff.PotentialCoachingAbility + " " + globalFuncs.localization.PlayerSearchLabels[ScoutLocalization.L_PA];

            this.worldrep = r.WorldReputation;
            this.nationalrep = r.NationalReputation;
            this.localrep = r.LocalReputation;

            int index = -1;
            this.depth = contexts[++index];
            this.directness = contexts[++index];
            this.flamboyancy = contexts[++index];
            this.flexibility = contexts[++index];
            this.freeroles = contexts[++index];
            this.marking = contexts[++index];
            this.offside = contexts[++index];
            this.pressing = contexts[++index];
            this.sittingback = contexts[++index];
            this.tempo = contexts[++index];
            this.useofplaymaker = contexts[++index];
            this.useofsubstitutions = contexts[++index];
            this.width = contexts[++index];
            this.adaptability = contexts[++index];
            this.ambition = contexts[++index];
            this.controversy = contexts[++index];
            this.determination = contexts[++index];
            this.loyalty = contexts[++index];
            this.pressure = contexts[++index];
            this.professionalism = contexts[++index];
            this.sportsmanship = contexts[++index];
            //this.temperament = contexts[++index];
            this.judgingplayerability = contexts[++index];
            this.judgingplayerpotential = contexts[++index];
            this.levelofdiscipline = contexts[++index];
            this.motivating = contexts[++index];
            this.physiotherapy = contexts[++index];
            this.tacticalknowledge = contexts[++index];
            this.attacking = contexts[++index];
            this.defending = contexts[++index];
            this.fitness = contexts[++index];
            this.goalkeepers = contexts[++index];
            this.mental = contexts[++index];
            this.player = contexts[++index];
            this.tactical = contexts[++index];
            this.technical = contexts[++index];
            this.manmanagement = contexts[++index];
            this.workingwithyoungsters = contexts[++index];
            this.buyingplayers = contexts[++index];
            this.hardnessoftraining = contexts[++index];
            this.mindgames = contexts[++index];
            this.squadrotation = contexts[++index];
            this.business = contexts[++index];
            this.intereference = contexts[++index];
            this.patience = contexts[++index];
            this.resources = contexts[++index];

			index = -1;
			CoachingRatings cr = (CoachingRatings)context.staffCRID[r.ID];
			this.rfitness = contextsratings[++index];
			this.rfitness.ImageSource = App.Current.Resources[cr.Fitness + "star"] as ImageSource;
			this.rgoalkeepers = contextsratings[++index];
			this.rgoalkeepers.ImageSource = App.Current.Resources[cr.Goalkeepers + "star"] as ImageSource;
			this.rtactics = contextsratings[++index];
			this.rtactics.ImageSource = App.Current.Resources[cr.Tactics + "star"] as ImageSource;
			this.rballcontrol = contextsratings[++index];
			this.rballcontrol.ImageSource = App.Current.Resources[cr.BallControl + "star"] as ImageSource;
			this.rdefending = contextsratings[++index];
			this.rdefending.ImageSource = App.Current.Resources[cr.Defending + "star"] as ImageSource;
			this.rattacking = contextsratings[++index];
			this.rattacking.ImageSource = App.Current.Resources[cr.Attacking + "star"] as ImageSource;
			this.rshooting = contextsratings[++index];
			this.rshooting.ImageSource = App.Current.Resources[cr.Shooting + "star"] as ImageSource;
			this.rsetpieces = contextsratings[++index];
			this.rsetpieces.ImageSource = App.Current.Resources[cr.SetPieces + "star"] as ImageSource;

            this.depth.TextBoxText = r.Depth;
            this.directness.TextBoxText = r.Directness;
            this.flamboyancy.TextBoxText = r.Flamboyancy;
            this.flexibility.TextBoxText = r.Flexibility;
            this.freeroles.TextBoxText = r.FreeRoles;
            this.marking.TextBoxText = r.Marking;
            this.offside.TextBoxText = r.Offside; 
            this.pressing.TextBoxText = r.Pressing;
            this.sittingback.TextBoxText = r.SittingBack;
            this.tempo.TextBoxText = r.Tempo;
            this.useofplaymaker.TextBoxText = r.UseOfPlaymaker;
            this.useofsubstitutions.TextBoxText = r.UseOfSubstitutions;
            this.width.TextBoxText = r.Width;
            this.adaptability.TextBoxText = r.Adaptability;
            this.ambition.TextBoxText = r.Ambition;
            this.controversy.TextBoxText = r.Controversy;
            this.determination.TextBoxText = r.Determination;
            this.loyalty.TextBoxText = r.Loyalty;
            this.pressure.TextBoxText = r.Pressure;
            this.professionalism.TextBoxText = r.Professionalism;
            this.sportsmanship.TextBoxText = r.Sportsmanship;
            //this.temperament.TextBoxText = r.Temperament;
            this.judgingplayerability.TextBoxText = r.JudgingPlayerAbility;
            this.judgingplayerpotential.TextBoxText = r.JudgingPlayerPotential;
            this.levelofdiscipline.TextBoxText = r.LevelOfDiscipline;
            this.motivating.TextBoxText = r.Motivating;
            this.physiotherapy.TextBoxText = r.Physiotherapy;
            this.tacticalknowledge.TextBoxText = r.TacticalKnowledge;
            this.attacking.TextBoxText = r.Attacking;
            this.defending.TextBoxText = r.Defending;
            this.fitness.TextBoxText = r.Fitness;
            this.goalkeepers.TextBoxText = r.Goalkeepers;
            this.mental.TextBoxText = r.Mental;
            this.player.TextBoxText = r.Player;
            this.tactical.TextBoxText = r.Tactical;
            this.technical.TextBoxText = r.Technical;
            this.manmanagement.TextBoxText = r.ManManagement;
            this.workingwithyoungsters.TextBoxText = r.WorkingWithYoungsters;
            this.buyingplayers.TextBoxText = r.BuyingPlayers;
            this.hardnessoftraining.TextBoxText = r.HardnessOfTraining;
            this.mindgames.TextBoxText = r.MindGames;
            this.squadrotation.TextBoxText = r.SquadRotation;
            this.business.TextBoxText = r.Business;
            this.intereference.TextBoxText = r.Interference;
            this.patience.TextBoxText = r.Patience;
            this.resources.TextBoxText = r.Resources;

            this.depth.TextBoxForeground = globalFuncs.setAttributeForeground(r.Depth);
            this.directness.TextBoxForeground = globalFuncs.setAttributeForeground(r.Directness);
            this.flamboyancy.TextBoxForeground = globalFuncs.setAttributeForeground(r.Flamboyancy);
            this.flexibility.TextBoxForeground = globalFuncs.setAttributeForeground(r.Flexibility);
            this.freeroles.TextBoxForeground = globalFuncs.setAttributeForeground(r.FreeRoles);
            this.marking.TextBoxForeground = globalFuncs.setAttributeForeground(r.Marking);
            this.offside.TextBoxForeground = globalFuncs.setAttributeForeground(r.Offside);
            this.pressing.TextBoxForeground = globalFuncs.setAttributeForeground(r.Pressing);
            this.sittingback.TextBoxForeground = globalFuncs.setAttributeForeground(r.SittingBack);
            this.tempo.TextBoxForeground = globalFuncs.setAttributeForeground(r.Tempo);
            this.useofplaymaker.TextBoxForeground = globalFuncs.setAttributeForeground(r.UseOfPlaymaker);
            this.useofsubstitutions.TextBoxForeground = globalFuncs.setAttributeForeground(r.UseOfSubstitutions);
            this.width.TextBoxForeground = globalFuncs.setAttributeForeground(r.Width);
            this.adaptability.TextBoxForeground = globalFuncs.setAttributeForeground(r.Adaptability);
            this.ambition.TextBoxForeground = globalFuncs.setAttributeForeground(r.Ambition);
            this.controversy.TextBoxForeground = globalFuncs.setAttributeForeground(r.Controversy);
            this.determination.TextBoxForeground = globalFuncs.setAttributeForeground(r.Determination);
            this.loyalty.TextBoxForeground = globalFuncs.setAttributeForeground(r.Loyalty);
            this.pressure.TextBoxForeground = globalFuncs.setAttributeForeground(r.Pressure);
            this.professionalism.TextBoxForeground = globalFuncs.setAttributeForeground(r.Professionalism);
            this.sportsmanship.TextBoxForeground = globalFuncs.setAttributeForeground(r.Sportsmanship);
            //this.temperament.TextBoxForeground = globalFuncs.setAttributeForeground(r.Temperament);
            this.judgingplayerability.TextBoxForeground = globalFuncs.setAttributeForeground(r.JudgingPlayerAbility);
            this.judgingplayerpotential.TextBoxForeground = globalFuncs.setAttributeForeground(r.JudgingPlayerPotential);
            this.levelofdiscipline.TextBoxForeground = globalFuncs.setAttributeForeground(r.LevelOfDiscipline);
            this.motivating.TextBoxForeground = globalFuncs.setAttributeForeground(r.Motivating);
            this.physiotherapy.TextBoxForeground = globalFuncs.setAttributeForeground(r.Physiotherapy);
            this.tacticalknowledge.TextBoxForeground = globalFuncs.setAttributeForeground(r.TacticalKnowledge);
            this.attacking.TextBoxForeground = globalFuncs.setAttributeForeground(r.Attacking);
            this.defending.TextBoxForeground = globalFuncs.setAttributeForeground(r.Defending);
            this.fitness.TextBoxForeground = globalFuncs.setAttributeForeground(r.Fitness);
            this.goalkeepers.TextBoxForeground = globalFuncs.setAttributeForeground(r.Goalkeepers);
            this.mental.TextBoxForeground = globalFuncs.setAttributeForeground(r.Mental);
            this.player.TextBoxForeground = globalFuncs.setAttributeForeground(r.Player);
            this.tactical.TextBoxForeground = globalFuncs.setAttributeForeground(r.Tactical);
            this.technical.TextBoxForeground = globalFuncs.setAttributeForeground(r.Technical);
            this.manmanagement.TextBoxForeground = globalFuncs.setAttributeForeground(r.ManManagement);
            this.workingwithyoungsters.TextBoxForeground = globalFuncs.setAttributeForeground(r.WorkingWithYoungsters);
            this.buyingplayers.TextBoxForeground = globalFuncs.setAttributeForeground(r.BuyingPlayers);
            this.hardnessoftraining.TextBoxForeground = globalFuncs.setAttributeForeground(r.HardnessOfTraining);
            this.mindgames.TextBoxForeground = globalFuncs.setAttributeForeground(r.MindGames);
            this.squadrotation.TextBoxForeground = globalFuncs.setAttributeForeground(r.SquadRotation);
            this.business.TextBoxForeground = globalFuncs.setAttributeForeground(r.Business);
            this.intereference.TextBoxForeground = globalFuncs.setAttributeForeground(r.Interference);
            this.patience.TextBoxForeground = globalFuncs.setAttributeForeground(r.Patience);
            this.resources.TextBoxForeground = globalFuncs.setAttributeForeground(r.Resources);
        }

        public void setLocalization()
        {
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> staffSearchAttributesGroupBoxes = localization.staffSearchAttributesGroupBoxes;

            int index = -1;
            this.tacticalattributes.Header = staffSearchAttributesGroupBoxes[++index];
            this.mentalattributes.Header = staffSearchAttributesGroupBoxes[++index];
            this.coachingattributes.Header = staffSearchAttributesGroupBoxes[++index];
            this.nontacticalattributes.Header = staffSearchAttributesGroupBoxes[++index];
            this.chairmanattributes.Header = staffSearchAttributesGroupBoxes[++index];
            this.personaldetails.Header = staffSearchAttributesGroupBoxes[++index];
            this.contractdetails.Header = staffSearchAttributesGroupBoxes[++index];
            this.ratings.Header = staffSearchAttributesGroupBoxes[++index];

            index = ScoutLocalization.SR_STAFFCOLUMNDEPTH;
            int counter = -1;
            for (int i = index; i < globalFuncs.localization.staffColumns.Count; ++i)
                contexts[++counter].LabelContent = globalFuncs.localization.staffColumns[i];

            ObservableCollection<String> StaffProfileLabels = localization.StaffProfileLabels;
            index = -1;
            this.contractstarted.LabelContent = StaffProfileLabels[++index];
            this.contractexpiring.LabelContent = StaffProfileLabels[++index];
            this.wage.LabelContent = StaffProfileLabels[++index];

            for (int i = 0; i < contextsratings.Count; ++i)
            {
                contextsratings[i].LabelContent = globalFuncs.localization.bestcrs[i + 1];
			}
        }
    }
}