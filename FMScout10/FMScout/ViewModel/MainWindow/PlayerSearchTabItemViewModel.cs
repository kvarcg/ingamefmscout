using System;
using FMScout.ControlContext;
using System.Windows.Media;

namespace FMScout.ViewModel
{
    public class PlayerSearchTabItemViewModel : SearchViewModel
    {
        public LabeledTextBoxContext fullName { get; set; }
        public LabeledTextBoxContext nation { get; set; }
        public LabeledTextBoxContext club { get; set; }
        public LabeledComboBoxContext region { get; set; }
        public LabeledNumericMinMaxContext age { get; set; }
        public LabeledNumericMinMaxContext ca { get; set; }
        public LabeledNumericMinMaxContext pa { get; set; }
        public LabeledNumericMinMaxContext pr { get; set; }
        public LabeledComboBoxContext bestpr { get; set; }
        public LabeledNumericMinMaxContext wage { get; set; }
        public LabeledNumericMinMaxContext value { get; set; }
        public LabeledNumericMinMaxContext saleValue { get; set; }
        public LabeledComboBoxContext contractStatus { get; set; }
        public LabeledComboBoxContext ownership { get; set; }
        public LabeledComboBoxContext eu { get; set; }
        public LabeledComboBoxContext regen { get; set; }
        public LabeledComboBoxContext prefFoot { get; set; }
        public LabeledHeaderContext positions { get; set; }
        public LabeledHeaderContext sides { get; set; }
        public LabeledHeaderContext GK { get; set; }
        public LabeledHeaderContext SW { get; set; }
        public LabeledHeaderContext D { get; set; }
        public LabeledHeaderContext WB { get; set; }
        public LabeledHeaderContext DM { get; set; }
        public LabeledHeaderContext M { get; set; }
        public LabeledHeaderContext AM { get; set; }
        public LabeledHeaderContext ST { get; set; }
        public LabeledHeaderContext left{ get; set; }
        public LabeledHeaderContext right{ get; set; }
        public LabeledHeaderContext center { get; set; }
        public LabeledHeaderContext free { get; set; }
    }

    public class PlayerGridViewModel : GridViewModel
    {
        public PLAYEREUSTATE EUState { get; set; }
        public PLAYERCLUBSTATE ClubState { get; set; }
        public bool S { get; set; }
        public ImageButtonContext imageButton { get; set; }
        public long ContractStartedTicks { get; set; }
        public long ContractExpiringTicks { get; set; }
        public String FullName { get; set; }
        public String Nation { get; set; }
        public String Club { get; set; }
        public String TeamSquad { get; set; }
        public String Position { get; set; }
        public int Age { get; set; }
        public int CA { get; set; }
        public int PA { get; set; }
        public int ADiff { get; set; }
        public String BestPR { get; set; }
        public String BestPRperc { get; set; }
        public int CurrentValue { get; set; }
        public int SaleValue { get; set; }
        public String ContractStarted { get; set; }
        public String ContractExpiring { get; set; }
        public int CurrentWage { get; set; }
        public int WorldReputation { get; set; }
        public int NationalReputation { get; set; }
        public int LocalReputation { get; set; }
        public int Corners { get; set; }
        public int Crossing { get; set; }
        public int Dribbling { get; set; }
        public int Finishing { get; set; }
        public int FirstTouch { get; set; }
        public int FreeKicks { get; set; }
        public int Heading { get; set; }
        public int LongShots { get; set; }
        public int LongThrows { get; set; }
        public int Marking { get; set; }
        public int Passing { get; set; }
        public int PenaltyTaking { get; set; }
        public int Tackling { get; set; }
        public int Technique { get; set; }
        public int Acceleration { get; set; }
        public int Agility { get; set; }
        public int Balance { get; set; }
        public int Jumping { get; set; }
        public int NaturalFitness { get; set; }
        public int Pace { get; set; }
        public int Stamina { get; set; }
        public int Strength { get; set; }
        public int LeftFoot { get; set; }
        public int RightFoot { get; set; }
        public int Aggression { get; set; }
        public int Anticipation { get; set; }
        public int Bravery { get; set; }
        public int Composure { get; set; }
        public int Concentration { get; set; }
        public int Creativity { get; set; }
        public int Decisions { get; set; }
        public int Determination { get; set; }
        public int Flair { get; set; }
        public int Influence { get; set; }
        public int OffTheBall { get; set; }
        public int Positioning { get; set; }
        public int TeamWork { get; set; }
        public int WorkRate { get; set; }
        public int Consistency { get; set; }
        public int Dirtyness { get; set; }
        public int ImportantMatches { get; set; }
        public int InjuryProneness { get; set; }
        public int Versatility { get; set; }
        public int AerialAbility { get; set; }
        public int CommandOfArea { get; set; }
        public int Communication { get; set; }
        public int Eccentricity { get; set; }
        public int Handling { get; set; }
        public int Kicking { get; set; }
        public int OneOnOnes { get; set; }
        public int Reflexes { get; set; }
        public int RushingOut { get; set; }
        public int TendencyToPunch { get; set; }
        public int Throwing { get; set; }
        public int Adaptability { get; set; }
        public int Ambition { get; set; }
        public int Controversy { get; set; }
        public int Loyalty { get; set; }
        public int Pressure { get; set; }
        public int Professionalism { get; set; }
        public int Sportsmanship { get; set; }
        public int Temperament { get; set; }
    }
}
