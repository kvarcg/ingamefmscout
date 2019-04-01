using System;
using System.Collections.Generic;
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
    public partial class StaffSearchAttributes : UserControl
    {
        private StaffSearchAttributesViewModel vm = null;
        private List<LabeledNumericMinMaxContext> contexts = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;

        public StaffSearchAttributes()
        {
            InitializeComponent();

            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs(); 
            setDataContext();
        }

        private void setNumericMinMaxContext(ref LabeledNumericMinMaxContext num, String name)
        {
            num.LabelContent = name;
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
            int index = ScoutLocalization.SR_STAFFCOLUMNDEPTH;

            contexts = new List<LabeledNumericMinMaxContext>();
            for (int i = index; i < globalFuncs.localization.staffColumns.Count; ++i)
            {
                LabeledNumericMinMaxContext context = new LabeledNumericMinMaxContext();
                setNumericMinMaxContext(ref context, globalFuncs.localization.staffColumns[i]);
                contexts.Add(context);
            }

            vm = new StaffSearchAttributesViewModel();

            vm.tacticalattributes = new LabeledHeaderContext();
            vm.mentalattributes = new LabeledHeaderContext();
            vm.coachingattributes = new LabeledHeaderContext();
            vm.nontacticalattributes = new LabeledHeaderContext();
            vm.chairmanattributes = new LabeledHeaderContext();

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

            setNumericUpDownMixMaxControlValue(ref this.depth);
            setNumericUpDownMixMaxControlValue(ref this.directness);
            setNumericUpDownMixMaxControlValue(ref this.flamboyancy);
            setNumericUpDownMixMaxControlValue(ref this.flexibility);
            setNumericUpDownMixMaxControlValue(ref this.freeroles);
            setNumericUpDownMixMaxControlValue(ref this.marking);
            setNumericUpDownMixMaxControlValue(ref this.offside);
            setNumericUpDownMixMaxControlValue(ref this.pressing);
            setNumericUpDownMixMaxControlValue(ref this.sittingback);
            setNumericUpDownMixMaxControlValue(ref this.tempo);
            setNumericUpDownMixMaxControlValue(ref this.useofplaymaker);
            setNumericUpDownMixMaxControlValue(ref this.useofsubstitutions);
            setNumericUpDownMixMaxControlValue(ref this.width);
            setNumericUpDownMixMaxControlValue(ref this.adaptability);
            setNumericUpDownMixMaxControlValue(ref this.ambition);
            setNumericUpDownMixMaxControlValue(ref this.controversy);
            setNumericUpDownMixMaxControlValue(ref this.determination);
            setNumericUpDownMixMaxControlValue(ref this.loyalty);
            setNumericUpDownMixMaxControlValue(ref this.pressure);
            setNumericUpDownMixMaxControlValue(ref this.professionalism);
            setNumericUpDownMixMaxControlValue(ref this.sportsmanship);
            //setNumericUpDownMixMaxControlValue(ref this.temperament);
            setNumericUpDownMixMaxControlValue(ref this.judgingplayerability);
            setNumericUpDownMixMaxControlValue(ref this.judgingplayerpotential);
            setNumericUpDownMixMaxControlValue(ref this.levelofdiscipline);
            setNumericUpDownMixMaxControlValue(ref this.motivating);
            setNumericUpDownMixMaxControlValue(ref this.physiotherapy);
            setNumericUpDownMixMaxControlValue(ref this.tacticalknowledge);
            setNumericUpDownMixMaxControlValue(ref this.attacking);
            setNumericUpDownMixMaxControlValue(ref this.defending);
            setNumericUpDownMixMaxControlValue(ref this.fitness);
            setNumericUpDownMixMaxControlValue(ref this.goalkeepers);
            setNumericUpDownMixMaxControlValue(ref this.mental);
            setNumericUpDownMixMaxControlValue(ref this.player);
            setNumericUpDownMixMaxControlValue(ref this.tactical);
            setNumericUpDownMixMaxControlValue(ref this.technical);
            setNumericUpDownMixMaxControlValue(ref this.manmanagement);
            setNumericUpDownMixMaxControlValue(ref this.workingwithyoungsters);
            setNumericUpDownMixMaxControlValue(ref this.buyingplayers);
            setNumericUpDownMixMaxControlValue(ref this.hardnessoftraining);
            setNumericUpDownMixMaxControlValue(ref this.mindgames);
            setNumericUpDownMixMaxControlValue(ref this.squadrotation);
            setNumericUpDownMixMaxControlValue(ref this.business);
            setNumericUpDownMixMaxControlValue(ref this.intereference);
            setNumericUpDownMixMaxControlValue(ref this.patience);
            setNumericUpDownMixMaxControlValue(ref this.resources);

            int index = -1;
            vm.depth = contexts[++index];
            vm.directness = contexts[++index];
            vm.flamboyancy = contexts[++index];
            vm.flexibility = contexts[++index];
            vm.freeroles = contexts[++index];
            vm.marking = contexts[++index];
            vm.offside = contexts[++index];
            vm.pressing = contexts[++index];
            vm.sittingback = contexts[++index];
            vm.tempo = contexts[++index];
            vm.useofplaymaker = contexts[++index];
            vm.useofsubstitutions = contexts[++index];
            vm.width = contexts[++index];
            vm.adaptability = contexts[++index];
            vm.ambition = contexts[++index];
            vm.controversy = contexts[++index];
            vm.determination = contexts[++index];
            vm.loyalty = contexts[++index];
            vm.pressure = contexts[++index];
            vm.professionalism = contexts[++index];
            vm.sportsmanship = contexts[++index];
            //vm.temperament = contexts[++index];
            vm.judgingplayerability = contexts[++index];
            vm.judgingplayerpotential = contexts[++index];
            vm.levelofdiscipline = contexts[++index];
            vm.motivating = contexts[++index];
            vm.physiotherapy = contexts[++index];
            vm.tacticalknowledge = contexts[++index];
            vm.attacking = contexts[++index];
            vm.defending = contexts[++index];
            vm.fitness = contexts[++index];
            vm.goalkeepers = contexts[++index];
            vm.mental = contexts[++index];
            vm.player = contexts[++index];
            vm.tactical = contexts[++index];
            vm.technical = contexts[++index];
            vm.manmanagement = contexts[++index];
            vm.workingwithyoungsters = contexts[++index];
            vm.buyingplayers = contexts[++index];
            vm.hardnessoftraining = contexts[++index];
            vm.mindgames = contexts[++index];
            vm.squadrotation = contexts[++index];
            vm.business = contexts[++index];
            vm.intereference = contexts[++index];
            vm.patience = contexts[++index];
            vm.resources = contexts[++index];
        }

        public void setLocalization()
        {
            ScoutLocalization localization = globalFuncs.localization;
            int index = -1;
            vm.tacticalattributes.Header = localization.staffSearchAttributesGroupBoxes[++index];
            vm.mentalattributes.Header = localization.staffSearchAttributesGroupBoxes[++index];
            vm.coachingattributes.Header = localization.staffSearchAttributesGroupBoxes[++index];
            vm.nontacticalattributes.Header = localization.staffSearchAttributesGroupBoxes[++index];
            vm.chairmanattributes.Header = localization.staffSearchAttributesGroupBoxes[++index];
        }

        public void clearData()
        {
            setControlValues();
        }
    }
}
            