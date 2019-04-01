using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FMScout.ControlContext;
using FMScout.ViewModel;
using FMScout.UserControls;

namespace FMScout.View.MainWindow
{
	public partial class PlayerSearchAttributes : UserControl
	{
        private PlayerSearchAttributesViewModel vm = null;
        private List<LabeledNumericMinMaxContext> contexts = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;

        public PlayerSearchAttributes()
		{
			this.InitializeComponent();
            
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs(); 
            setDataContext();
        }

        private void setNumericMinMaxContext(ref LabeledNumericMinMaxContext num)
        {
            num.LabelWidth = 110;
            num.LabelHeight = 20;
            num.Minimum = 0; 
            num.Maximum = 20;
            num.NumericUpDownMinMaxHeight = 16;
            num.NumericUpDownMinMaxWidth = 50;
        }

        private void setNumericMinMaxControlValue(ref LabeledNumericMinMaxContext num)
        {
            num.ValueMin = 0;
            num.ValueMax = 20;
        }

        private void setNumericUpDownMixMaxControlValue(ref LabeledNumericUpDownMinMaxAttributes num)
        {
            num.NumericUpDownMin.Value = 0;
            num.NumericUpDownMax.Value = 20;
        }

        public void setDataContext()
        {
            int index = ScoutLocalization.SR_PLAYERCOLUMNSCORNER;

            contexts = new List<LabeledNumericMinMaxContext>();
            for (int i = index; i < globalFuncs.localization.playerColumns.Count; ++i)
            {
                LabeledNumericMinMaxContext context = new LabeledNumericMinMaxContext();
                setNumericMinMaxContext(ref context);
                contexts.Add(context);
            }

            vm = new PlayerSearchAttributesViewModel();

            vm.technicalattributes = new LabeledHeaderContext();
            vm.physicalattributes = new LabeledHeaderContext();
            vm.mentalattributes = new LabeledHeaderContext();
            vm.hiddenattributes = new LabeledHeaderContext();
            vm.goalkeepingattributes = new LabeledHeaderContext();
            vm.mentaltraitsattributes = new LabeledHeaderContext();
           
            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues()
        {
            for (int i = 0; i < contexts.Count; ++i)
            {
                LabeledNumericMinMaxContext context = contexts[i];
                setNumericMinMaxControlValue(ref context);
            }

            setNumericUpDownMixMaxControlValue(ref this.corners);
            setNumericUpDownMixMaxControlValue(ref this.crossing);
            setNumericUpDownMixMaxControlValue(ref this.dribbling);
            setNumericUpDownMixMaxControlValue(ref this.finishing);
            setNumericUpDownMixMaxControlValue(ref this.firsttouch);
            setNumericUpDownMixMaxControlValue(ref this.freekicks);
            setNumericUpDownMixMaxControlValue(ref this.heading);
            setNumericUpDownMixMaxControlValue(ref this.longshots);
            setNumericUpDownMixMaxControlValue(ref this.longthrows);
            setNumericUpDownMixMaxControlValue(ref this.marking);
            setNumericUpDownMixMaxControlValue(ref this.passing);
            setNumericUpDownMixMaxControlValue(ref this.penaltytaking);
            setNumericUpDownMixMaxControlValue(ref this.tackling);
            setNumericUpDownMixMaxControlValue(ref this.technique);
            setNumericUpDownMixMaxControlValue(ref this.acceleration);
            setNumericUpDownMixMaxControlValue(ref this.agility);
            setNumericUpDownMixMaxControlValue(ref this.balance);
            setNumericUpDownMixMaxControlValue(ref this.jumping);
            setNumericUpDownMixMaxControlValue(ref this.naturalfitness);
            setNumericUpDownMixMaxControlValue(ref this.pace);
            setNumericUpDownMixMaxControlValue(ref this.stamina);
            setNumericUpDownMixMaxControlValue(ref this.strength);
            setNumericUpDownMixMaxControlValue(ref this.leftfoot);
            setNumericUpDownMixMaxControlValue(ref this.rightfoot);
            setNumericUpDownMixMaxControlValue(ref this.aggression);
            setNumericUpDownMixMaxControlValue(ref this.anticipation);
            setNumericUpDownMixMaxControlValue(ref this.bravery);
            setNumericUpDownMixMaxControlValue(ref this.composure);
            setNumericUpDownMixMaxControlValue(ref this.concentration);
            setNumericUpDownMixMaxControlValue(ref this.creativity);
            setNumericUpDownMixMaxControlValue(ref this.decisions);
            setNumericUpDownMixMaxControlValue(ref this.determination);
            setNumericUpDownMixMaxControlValue(ref this.flair);
            setNumericUpDownMixMaxControlValue(ref this.influence);
            setNumericUpDownMixMaxControlValue(ref this.offtheball);
            setNumericUpDownMixMaxControlValue(ref this.positioning);
            setNumericUpDownMixMaxControlValue(ref this.teamwork);
            setNumericUpDownMixMaxControlValue(ref this.workrate);
            setNumericUpDownMixMaxControlValue(ref this.consistency);
            setNumericUpDownMixMaxControlValue(ref this.dirtyness);
            setNumericUpDownMixMaxControlValue(ref this.importantmatches);
            setNumericUpDownMixMaxControlValue(ref this.injuryproneness);
            setNumericUpDownMixMaxControlValue(ref this.versatility);
            setNumericUpDownMixMaxControlValue(ref this.aerialability);
            setNumericUpDownMixMaxControlValue(ref this.commandofarea);
            setNumericUpDownMixMaxControlValue(ref this.communication);
            setNumericUpDownMixMaxControlValue(ref this.eccentricity);
            setNumericUpDownMixMaxControlValue(ref this.handling);
            setNumericUpDownMixMaxControlValue(ref this.kicking);
            setNumericUpDownMixMaxControlValue(ref this.oneonones);
            setNumericUpDownMixMaxControlValue(ref this.reflexes);
            setNumericUpDownMixMaxControlValue(ref this.rushingout);
            setNumericUpDownMixMaxControlValue(ref this.tendencytopunch);
            setNumericUpDownMixMaxControlValue(ref this.throwing);
            setNumericUpDownMixMaxControlValue(ref this.adaptability);
            setNumericUpDownMixMaxControlValue(ref this.ambition);
            setNumericUpDownMixMaxControlValue(ref this.controversy);
            setNumericUpDownMixMaxControlValue(ref this.loyalty);
            setNumericUpDownMixMaxControlValue(ref this.pressure);
            setNumericUpDownMixMaxControlValue(ref this.professionalism);
            setNumericUpDownMixMaxControlValue(ref this.sportsmanship);
            setNumericUpDownMixMaxControlValue(ref this.temperament);

            int index = -1;
            vm.corners = contexts[++index];
            vm.crossing = contexts[++index];
            vm.dribbling = contexts[++index];
            vm.finishing = contexts[++index];
            vm.firsttouch = contexts[++index];
            vm.freekicks = contexts[++index];
            vm.heading = contexts[++index];
            vm.longshots = contexts[++index];
            vm.longthrows = contexts[++index];
            vm.marking = contexts[++index];
            vm.passing = contexts[++index];
            vm.penaltytaking = contexts[++index];
            vm.tackling = contexts[++index];
            vm.technique = contexts[++index];
            vm.acceleration = contexts[++index];
            vm.agility = contexts[++index];
            vm.balance = contexts[++index];
            vm.jumping = contexts[++index];
            vm.naturalfitness = contexts[++index];
            vm.pace = contexts[++index];
            vm.stamina = contexts[++index];
            vm.strength = contexts[++index];
            vm.leftfoot = contexts[++index];
            vm.rightfoot = contexts[++index];
            vm.aggression = contexts[++index];
            vm.anticipation = contexts[++index];
            vm.bravery = contexts[++index];
            vm.composure = contexts[++index];
            vm.concentration = contexts[++index];
            vm.creativity = contexts[++index];
            vm.decisions = contexts[++index];
            vm.determination = contexts[++index];
            vm.flair = contexts[++index];
            vm.influence = contexts[++index];
            vm.offtheball = contexts[++index];
            vm.positioning = contexts[++index];
            vm.teamwork = contexts[++index];
            vm.workrate = contexts[++index];
            vm.consistency = contexts[++index];
            vm.dirtyness = contexts[++index];
            vm.importantmatches = contexts[++index];
            vm.injuryproneness = contexts[++index];
            vm.versatility = contexts[++index];
            vm.aerialability = contexts[++index];
            vm.commandofarea = contexts[++index];
            vm.communication = contexts[++index];
            vm.eccentricity = contexts[++index];
            vm.handling = contexts[++index];
            vm.kicking = contexts[++index];
            vm.oneonones = contexts[++index];
            vm.reflexes = contexts[++index];
            vm.rushingout = contexts[++index];
            vm.tendencytopunch = contexts[++index];
            vm.throwing = contexts[++index];
            vm.adaptability = contexts[++index];
            vm.ambition = contexts[++index];
            vm.controversy = contexts[++index];
            vm.loyalty = contexts[++index];
            vm.pressure = contexts[++index];
            vm.professionalism = contexts[++index];
            vm.sportsmanship = contexts[++index];
            vm.temperament = contexts[++index];
        }

        public void setLocalization()
        {
            int index = ScoutLocalization.SR_PLAYERCOLUMNSCORNER;
            int counter = -1;
            for (int i = index; i < globalFuncs.localization.playerColumns.Count; ++i)
                contexts[++counter].LabelContent = globalFuncs.localization.playerColumns[i];
            
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> playerSearchAttributesGroupBoxes = localization.playerSearchAttributesGroupBoxes;
            index = -1; 
            vm.technicalattributes.Header = playerSearchAttributesGroupBoxes[++index];
            vm.physicalattributes.Header = playerSearchAttributesGroupBoxes[++index];
            vm.mentalattributes.Header = playerSearchAttributesGroupBoxes[++index];
            vm.hiddenattributes.Header = playerSearchAttributesGroupBoxes[++index];
            vm.goalkeepingattributes.Header = playerSearchAttributesGroupBoxes[++index];
            vm.mentaltraitsattributes.Header = playerSearchAttributesGroupBoxes[++index];
        }

        public void clearData()
        {
            setControlValues();
        }
	}
}