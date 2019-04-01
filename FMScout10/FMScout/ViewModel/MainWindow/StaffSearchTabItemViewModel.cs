using System;
using System.Windows.Media;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class StaffSearchTabItemViewModel : SearchViewModel
    {
        public LabeledTextBoxContext fullName { get; set; }
        public LabeledTextBoxContext nation { get; set; }
        public LabeledTextBoxContext club { get; set; }
        public LabeledComboBoxContext role { get; set; }
        public LabeledComboBoxContext region { get; set; }
        public LabeledNumericMinMaxContext age { get; set; }
        public LabeledNumericMinMaxContext ca { get; set; }
        public LabeledNumericMinMaxContext pa { get; set; }
        public LabeledComboBoxContext contractStatus { get; set; }
        public LabeledNumericMinMaxContext fitness { get; set; }
        public LabeledNumericMinMaxContext goalkeepers { get; set; }
        public LabeledNumericMinMaxContext tactics { get; set; }
        public LabeledNumericMinMaxContext ballControl { get; set; }
        public LabeledNumericMinMaxContext defending { get; set; }
        public LabeledNumericMinMaxContext attacking { get; set; }
        public LabeledNumericMinMaxContext shooting { get; set; }
        public LabeledNumericMinMaxContext setPieces { get; set; }
        public LabeledComboBoxContext bestcr { get; set; }
        public LabeledComboBoxContext regen { get; set; }
    }

    public class StaffGridViewModel : GridViewModel
    {
        public STAFFCLUBSTATE ClubState { get; set; }
        public String FullName { get; set; }
        public String Nation { get; set; }
        public String Club { get; set; }
        public String Role { get; set; }
        public int Age { get; set; }
        public int CA { get; set; }
        public int PA { get; set; }
        public int ADiff { get; set; }
        public String BestCR { get; set; }
        public int BestCRStars { get; set; }
        public ImageSource BestCRStarsImage { get; set; }
        public String ContractStarted { get; set; }
        public long ContractStartedTicks { get; set; }
        public String ContractExpiring { get; set; }
        public long ContractExpiringTicks { get; set; }
        public int CurrentWage { get; set; }
        public int WorldReputation { get; set; }
        public int NationalReputation { get; set; }
        public int LocalReputation { get; set; }
        public int Depth { get; set; }
        public int Directness { get; set; }
        public int Flamboyancy { get; set; }
        public int Flexibility { get; set; }
        public int FreeRoles { get; set; }
        public int Marking { get; set; }
        public int Offside { get; set; }
        public int Pressing { get; set; }
        public int SittingBack { get; set; }
        public int Tempo { get; set; }
        public int UseOfPlaymaker { get; set; }
        public int UseOfSubstitutions { get; set; }
        public int Width { get; set; }
        public int Adaptability { get; set; }
        public int Ambition { get; set; }
        public int Controversy { get; set; }
        public int Determination { get; set; }
        public int Loyalty { get; set; }
        public int Pressure { get; set; }
        public int Professionalism { get; set; }
        public int Sportsmanship { get; set; }
        //public int Temperament { get; set; }
        public int JudgingPlayerAbility { get; set; }
        public int JudgingPlayerPotential { get; set; }
        public int LevelOfDiscipline { get; set; }
        public int Motivating { get; set; }
        public int Physiotherapy { get; set; }
        public int TacticalKnowledge { get; set; }
        public int Attacking { get; set; }
        public int Defending { get; set; }
        public int Fitness { get; set; }
        public int Goalkeepers { get; set; }
        public int Mental { get; set; }
        public int Player { get; set; }
        public int Tactical { get; set; }
        public int Technical { get; set; }
        public int ManManagement { get; set; }
        public int WorkingWithYoungsters { get; set; }
        public int BuyingPlayers { get; set; }
        public int HardnessOfTraining { get; set; }
        public int MindGames { get; set; }
        public int SquadRotation { get; set; }
        public int Business { get; set; }
        public int Interference { get; set; }
        public int Patience { get; set; }
        public int Resources { get; set; }
    }
}