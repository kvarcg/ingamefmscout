using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Young3.FMSearch.Business.Entities.InGame;

namespace FMScout
{
    public partial class MiniScoutForm : Form
    {
        private MainForm mainForm = null;
        private Player player = null;
        private Staff staff = null;
        private Team team = null;

        public MiniScoutForm(bool loadthis)
        {
            InitializeComponent();
            mainForm = new MainForm(false);
            clear();
            this.OnTopToolStripButton.Image = global::FMScout.Properties.Resources.OnTopDisabled;
            this.OnTopToolStripButton.Text = "On Top: No";
            this.OnTopToolStripButton.ToolTipText = "On Top: No";
            setColors();
            this.PlayerHiddenDetailsSplitter.ToggleSplitter2();
            this.StaffHiddenDetailsSplitter.ToggleSplitter2();
            PlayerHiddenDetailsSplitter_MouseClick(null, null);
            StaffHiddenDetailsSplitter_MouseClick(null, null);
            this.Size = new Size(268, 446);
            selectPlayerInfo();
            reset(false);
            this.InfoToolTip.ToolTipTitle = "Ingame MiniScout " + FMScout.MainForm.CurrentVersion;
            this.InfoToolTip.SetToolTip(this.InfoPictureBox, this.mainForm.infoText(false));
        }

        public MiniScoutForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            clear();
            this.StaffHiddenDetailsSplitter.currentState = FMScout.Controls.SplitterState.Expanded;
            this.PlayerHiddenDetailsSplitter.currentState = FMScout.Controls.SplitterState.Expanded;
            this.OnTopToolStripButton.Image = global::FMScout.Properties.Resources.OnTopDisabled;
            this.OnTopToolStripButton.Text = "On Top: No";
            this.OnTopToolStripButton.ToolTipText = "On Top: No";
            this.PlayerInfoGroupBox.Visible = true;
            this.StaffInfoGroupBox.Visible = false;
            this.TeamInfoGroupBox.Visible = false;
            loadMiniScout();
        }

        public void ShowMiniScout()
        {
            this.Show(mainForm);
            loadMiniScout();            
        }

        private void LoadFM2009StripButton_Click(object sender, EventArgs e)
        {
            clear();
            this.InfoToolTip.ToolTipTitle = "Ingame MiniScout " + FMScout.MainForm.CurrentVersion;
            if (this.mainForm.loadMiniScoutFM())
            {
                reset(true);
                this.InfoToolTip.SetToolTip(this.InfoPictureBox, this.mainForm.infoText(true));
            }
            else
            {
                reset(false);
                this.InfoToolTip.SetToolTip(this.InfoPictureBox, this.mainForm.infoText(true));
            }
        }

        public void reset(bool enabled)
        {
            this.RefreshToolStripButton.Enabled = enabled;
            this.ClearToolStripButton.Enabled = enabled;
            this.OnTopToolStripButton.Enabled = enabled;
            this.PlayerToolStripButton.Enabled = enabled;
            this.StaffToolStripButton.Enabled = enabled;
            this.TeamToolStripButton.Enabled = enabled;
            this.PlayerInfoGroupBox.Enabled = enabled;
            this.StaffInfoGroupBox.Enabled = enabled;
            this.TeamInfoGroupBox.Enabled = enabled;
        }

        public void loadMiniScout()
        {
            if (this.StaffHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Expanded)
            {
                this.StaffInfoGroupBox.Visible = true;
                this.StaffHiddenPanel.Visible = true;
                this.StaffHiddenDetailsSplitter.ToggleSplitter();
                StaffHiddenDetailsSplitter_MouseClick(null, null);
                selectStaffInfo();
            }
            if (this.PlayerHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Expanded)
            {
                this.PlayerInfoGroupBox.Visible = true;
                this.PlayerHiddenPanel.Visible = true;
                this.PlayerHiddenDetailsSplitter.ToggleSplitter();
                PlayerHiddenDetailsSplitter_MouseClick(null, null);
                selectPlayerInfo();
            }
        }

        public void clear()
        {
            clearPlayer();
            clearStaff();
            clearTeam();
        }

        public void clearPlayer()
        {
            // personal details
            this.PlayerProfileIDTextBox.Text = "ID";
            this.PlayerProfileFullNameTextBox.Text = "Full Name";
            this.PlayerProfileClubTextBox.Text = "Club";
            this.PlayerProfileContractExpiryTextBox.Text = "Contract Expiry Date";
            this.PlayerProfileValueTextBox.Text = "Value";
            this.PlayerProfileSaleValueTextBox.Text = "Sale Value";

            this.PlayerProfileLocalReputationTextBox.Text = "0";
            this.PlayerProfileNationalReputationTextBox.Text = "0";
            this.PlayerProfileWorldReputationTextBox.Text = "0";
            this.PlayerProfileCATextBox.Text = "CA";
            this.PlayerProfilePATextBox.Text = "PA";
            this.PlayerProfilePositionTextBox.Text = "Position";

            // hidden skills
            this.PlayerProfileConsistencyTextBox.Text = "0";
            this.PlayerProfileDirtynessTextBox.Text = "0";
            this.PlayerProfileImportantMatchesTextBox.Text = "0";
            this.PlayerProfileInjuryPronenessTextBox.Text = "0";
            this.PlayerProfileVersatilityTextBox.Text = "0";

            // mental traits skills
            this.PlayerProfileAdaptabilityTextBox.Text = "0";
            this.PlayerProfileAmbitionTextBox.Text = "0";
            this.PlayerProfileControversyTextBox.Text = "0";
            this.PlayerProfileLoyaltyTextBox.Text = "0";
            this.PlayerProfilePressureTextBox.Text = "0";
            this.PlayerProfileProfessionalismTextBox.Text = "0";
            this.PlayerProfileSportsmanshipTextBox.Text = "0";
            this.PlayerProfileTemperamentTextBox.Text = "0";

            // positional ratings
            this.PlayerProfilePositionalRatingTextBox.Text = "None";
        }

        public void clearStaff()
        {
            this.StaffProfileIDTextBox.Text = "ID"; 
            this.StaffProfileFullNameTextBox.Text = "Full Name";
            this.StaffProfileClubTextBox.Text = "Club";
            this.StaffProfileContractExpiryTextBox.Text = "Contract Expiry Date";
            this.StaffProfileWageTextBox.Text = "0";
            this.StaffProfileCATextBox.Text = "CA";
            this.StaffProfilePATextBox.Text = "PA";
            this.StaffProfileRoleTextBox.Text = "Role";
            this.StaffProfileReputationTextBox.Text = "Reputation";
            this.StaffProfileDepthTextBox.Text = "0";
            this.StaffProfileDirectnessTextBox.Text = "0";
            this.StaffProfileFlamboyancyTextBox.Text = "0";
            this.StaffProfileFlexibilityTextBox.Text = "0";
            this.StaffProfileFreeRolesTextBox.Text = "0";
            this.StaffProfileMarkingTextBox.Text = "0";
            this.StaffProfileOffsideTextBox.Text = "0";
            this.StaffProfilePressingTextBox.Text = "0";
            this.StaffProfileSittingBackTextBox.Text = "0";
            this.StaffProfileTempoTextBox.Text = "0";
            this.StaffProfileUseOfPlaymakerTextBox.Text = "0";
            this.StaffProfileUseOfSubstitutionsTextBox.Text = "0";
            this.StaffProfileWidthTextBox.Text = "0";
            this.StaffProfileBuyingPlayersTextBox.Text = "0";
            this.StaffProfileHardnessOfTrainingTextBox.Text = "0";
            this.StaffProfileMindGamesTextBox.Text = "0";
            this.StaffProfileSquadRotationTextBox.Text = "0";
            this.StaffProfileBusinessTextBox.Text = "0";
            this.StaffProfileInterferenceTextBox.Text = "0";
            this.StaffProfilePatienceTextBox.Text = "0";
            this.StaffProfileResourcesTextBox.Text = "0";
            this.StaffProfileBestRatingsGroupBox.Refresh();
        }

        public void clearTeam()
        {
            this.TeamProfileIDTextBox.Text = "ID";
            this.TeamProfileNameTextBox.Text = "Name";
            this.TeamProfileTrainingGroundTextBox.Text = "0";
            this.TeamProfileYouthGroundTextBox.Text = "0";
            this.TeamProfileYouthAcademyTextBox.Text = "None";
            this.TeamProfileReputationTextBox.Text = "0";

            // finance details
            this.TeamProfileTotalTransferTextBox.Text = "0";
            this.TeamProfileRemainingTransferTextBox.Text = "0";
            this.TeamProfileBalanceTextBox.Text = "0";
            this.TeamProfileTotalWageTextBox.Text = "0";
            this.TeamProfileUsedWageTextBox.Text = "0";
        }

        public void setColors()
        {
            this.BackColor = this.mainForm.colorMain;
            this.ForeColor = this.mainForm.colorMainText;
            this.PlayerHiddenDetailsSplitter.BackColor = this.mainForm.colorMain;
            this.StaffHiddenDetailsSplitter.BackColor = this.mainForm.colorMain;
            this.StaffMoreHiddenDetailsSplitter.BackColor = this.mainForm.colorMain;
            this.PlayerBasicPanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.PlayerBasicPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.PlayerHiddenPanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.PlayerHiddenPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.StaffBasicPanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.StaffBasicPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.StaffHiddenPanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.StaffHiddenPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.StaffMoreHiddenPanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.StaffMoreHiddenPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.TeamPanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.TeamPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.Font = this.mainForm.themeFont;
            this.MainToolStrip.BackColor = this.mainForm.colorToolbar;
            this.MainToolStrip.ForeColor = this.mainForm.colorToolbar;
            this.InfoPictureBox.BackColor = this.mainForm.colorToolbar;
            setPlayerColors();
            setStaffColors();
            setTeamColors();
        }

        public void setPlayerColors()
        {
            this.PlayerBasicPanel.BackColor = this.mainForm.colorPanels;
            this.PlayerHiddenPanel.BackColor = this.mainForm.colorPanels;
            this.mainForm.colorProfileGroupBox(ref this.PlayerInfoGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.PlayerBasicInfoGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.PlayerPositionalRatingGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.PlayerProfileReputationGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.PlayerProfileHiddenSkillsGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.PlayerProfileMentalTraitsSkillsGroupBox);

            this.Font = this.mainForm.themeFont;

            // personal details
            this.PlayerProfileIDTextBox.BackColor = this.mainForm.colorProfileFields1;
            this.PlayerProfileIDTextBox.ForeColor = this.mainForm.colorProfileFieldsText;
            this.PlayerProfileFullNameTextBox.BackColor = this.mainForm.colorProfileFields2;
            this.PlayerProfileFullNameTextBox.ForeColor = this.mainForm.colorProfileFieldsText;
            this.mainForm.colorInfo(ref this.PlayerProfileClubTextBox);
            this.mainForm.colorInfo(ref this.PlayerProfileContractExpiryTextBox);
            this.mainForm.colorInfo(ref this.PlayerProfileValueTextBox);
            this.mainForm.colorInfo(ref this.PlayerProfileSaleValueTextBox);

            this.mainForm.colorInfo(ref this.PlayerProfileLocalReputationTextBox);
            this.mainForm.colorInfo(ref this.PlayerProfileNationalReputationTextBox);
            this.mainForm.colorInfo(ref this.PlayerProfileWorldReputationTextBox);
            this.mainForm.colorInfo(ref this.PlayerProfileCATextBox);
            this.mainForm.colorInfo(ref this.PlayerProfilePATextBox);
            this.mainForm.colorInfo(ref this.PlayerProfilePositionTextBox);

            // hidden skills
            this.mainForm.colorAttribute(ref this.PlayerProfileConsistencyTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileDirtynessTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileImportantMatchesTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileInjuryPronenessTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileVersatilityTextBox);

            // mental traits skills
            this.mainForm.colorAttribute(ref this.PlayerProfileAdaptabilityTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileAmbitionTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileControversyTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileLoyaltyTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfilePressureTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileProfessionalismTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileSportsmanshipTextBox);
            this.mainForm.colorAttribute(ref this.PlayerProfileTemperamentTextBox);

            // positional ratings
            this.mainForm.colorInfo(ref this.PlayerProfilePositionalRatingTextBox);
        }

        public void setStaffColors()
        {
            this.StaffBasicPanel.BackColor = this.mainForm.colorPanels;
            this.StaffHiddenPanel.BackColor = this.mainForm.colorPanels;
            this.StaffMoreHiddenPanel.BackColor = this.mainForm.colorPanels;
            this.mainForm.colorProfileGroupBox(ref this.StaffInfoGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.StaffBasicInfoGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.StaffProfileBestRatingsGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.StaffProfileChairmanSkillsGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.StaffProfileNonTacticalSkillsGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.StaffProfileTacticalSkillsGroupBox);

            // personal details
            this.mainForm.colorInfo(ref this.StaffProfileIDTextBox);
            this.mainForm.colorInfo(ref this.StaffProfileFullNameTextBox);
            this.mainForm.colorInfo(ref this.StaffProfileClubTextBox);
            this.mainForm.colorInfo(ref this.StaffProfileContractExpiryTextBox);
            this.mainForm.colorInfo(ref this.StaffProfileWageTextBox);
            this.mainForm.colorInfo(ref this.StaffProfileCATextBox);
            this.mainForm.colorInfo(ref this.StaffProfilePATextBox);
            this.mainForm.colorInfo(ref this.StaffProfileRoleTextBox);
            this.mainForm.colorInfo(ref this.StaffProfileReputationTextBox);

            // ratings
            this.mainForm.colorInfo(ref this.StaffProfileRatingsFitnessLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsGoalkeepersLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsBallControlLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsTacticsLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsDefendingLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsAttackingLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsShootingLabel);
            this.mainForm.colorInfo(ref this.StaffProfileRatingsSetPiecesLabel);

            // tactical skills
            this.mainForm.colorAttribute(ref this.StaffProfileDepthTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileDirectnessTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileFlamboyancyTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileFlexibilityTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileFreeRolesTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileMarkingTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileOffsideTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfilePressingTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileSittingBackTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileTempoTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileUseOfPlaymakerTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileUseOfSubstitutionsTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileWidthTextBox);

            // non tactical skills
            this.mainForm.colorAttribute(ref this.StaffProfileBuyingPlayersTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileHardnessOfTrainingTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileMindGamesTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileSquadRotationTextBox);

            // chairman skills
            this.mainForm.colorAttribute(ref this.StaffProfileBusinessTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileInterferenceTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfilePatienceTextBox);
            this.mainForm.colorAttribute(ref this.StaffProfileResourcesTextBox);
        }

        public void setTeamColors()
        {
            this.TeamPanel.BackColor = this.mainForm.colorPanels;
            this.mainForm.colorProfileGroupBox(ref this.TeamInfoGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.TeamBasicInfoGroupBox);
            this.mainForm.colorProfileGroupBox(ref this.TeamProfileFinanceDetailsGroupBox);

            // general details
            this.mainForm.colorInfo(ref this.TeamProfileIDTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileNameTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileTrainingGroundTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileYouthGroundTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileYouthAcademyTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileReputationTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileTotalTransferTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileRemainingTransferTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileBalanceTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileTotalWageTextBox);
            this.mainForm.colorInfo(ref this.TeamProfileUsedWageTextBox);
        }

        private void PlayerHiddenDetailsSplitter_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.PlayerInfoGroupBox.Size.Height == 271)
                this.PlayerInfoGroupBox.Size = new Size(226, 727);
            else
                this.PlayerInfoGroupBox.Size = new Size(226, 271);
        }

        private void StaffHiddenDetailsSplitter_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.StaffInfoGroupBox.Size.Height == 248)
            {
                if (this.StaffMoreHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Collapsed)
                {
                    this.StaffHiddenPanel.Size = new Size(220, 202);
                    this.StaffInfoGroupBox.Size = new Size(226, 450);
                    this.Size = new Size(268, 520);
                }
                else if (this.StaffMoreHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Expanded)
                {
                    this.StaffMoreHiddenPanel.Size = new Size(220, 518);
                    this.StaffHiddenPanel.Size = new Size(220, 722);
                    this.StaffInfoGroupBox.Size = new Size(226, 970);
                    this.Size = new Size(268, 1038);
                }
            }
            else
            {
                this.StaffInfoGroupBox.Size = new Size(226, 248);
                this.Size = new Size(268, 318);
            }
        }

        private void StaffMoreHiddenDetailsSplitter_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.StaffInfoGroupBox.Size.Height == 450)
            {
                this.StaffHiddenPanel.Size = new Size(220, 722);
                this.StaffInfoGroupBox.Size = new Size(226, 970);
                this.Size = new Size(268, 1038);
            }
            else
            {
                this.StaffHiddenPanel.Size = new Size(220, 202);
                this.StaffInfoGroupBox.Size = new Size(226, 450);
                this.Size = new Size(268, 520);
            }
        }

        private void PlayerToolStripButton_Click(object sender, EventArgs e)
        {
            selectPlayerInfo();
        }

        private void StaffToolStripButton_Click(object sender, EventArgs e)
        {
            selectStaffInfo();
        }

        private void TeamToolStripButton_Click(object sender, EventArgs e)
        {
            selectTeamInfo();
        }

        private void selectPlayerInfo()
        {
            this.PlayerInfoGroupBox.Visible = true;
            this.StaffInfoGroupBox.Visible = false;
            this.TeamInfoGroupBox.Visible = false;
            if (this.PlayerHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Collapsed)
                this.Size = new Size(268, 341);
            else if (this.PlayerHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Expanded)
                this.Size = new Size(268, 794);
        }

        private void selectStaffInfo()
        {
            this.PlayerInfoGroupBox.Visible = false;
            this.StaffInfoGroupBox.Visible = true;
            this.TeamInfoGroupBox.Visible = false;
            if (this.StaffHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Collapsed)
                this.Size = new Size(268, 318);
            else if (this.StaffHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Expanded)
            {
                if (this.StaffMoreHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Collapsed)
                {
                    this.StaffHiddenPanel.Size = new Size(220, 202);
                    this.StaffInfoGroupBox.Size = new Size(226, 450);
                    this.Size = new Size(268, 520);
                }
                else if (this.StaffMoreHiddenDetailsSplitter.currentState == FMScout.Controls.SplitterState.Expanded)
                {
                    this.StaffMoreHiddenPanel.Size = new Size(220, 518);
                    this.StaffHiddenPanel.Size = new Size(220, 722);
                    this.StaffInfoGroupBox.Size = new Size(226, 970);
                    this.Size = new Size(268, 1038);
                }
            }
        }


        private void selectTeamInfo()
        { 
            this.PlayerInfoGroupBox.Visible = false;
            this.StaffInfoGroupBox.Visible = false;
            this.TeamInfoGroupBox.Visible = true;
            this.Size = new Size(268, 371);
        }

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            player = this.mainForm.ActivePlayerObject();
            staff = this.mainForm.ActiveStaffObject();
            team = this.mainForm.ActiveTeamObject();
            if (player != null)
            {
                selectPlayerInfo();

                this.PlayerProfileIDTextBox.Text = player.ID.ToString();
                this.PlayerProfileFullNameTextBox.Text = player.ToString();
                if (player.Nickname.Length > 0)
                    this.PlayerProfileFullNameTextBox.Text += " (" + player.Nickname + ")";
                this.PlayerProfileValueTextBox.Text = "Value: " + (player.Value * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
                if (player.SaleValue != -1)
                    this.PlayerProfileSaleValueTextBox.Text = "Sale Value: " + (player.SaleValue * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
                else
                    this.PlayerProfileSaleValueTextBox.Text = "No Sale Value";
                this.PlayerProfileCATextBox.Text = player.CurrentPlayingAbility.ToString() + " CA";
                this.PlayerProfilePATextBox.Text = player.PotentialPlayingAbility.ToString() + " PA";
                string playerPosition = "";
                List<string> positions = new List<string>();
                List<string> sides = new List<string>();
                this.mainForm.find_player_position(player, ref playerPosition, ref positions, ref sides, true);
                this.PlayerProfilePositionTextBox.Text = playerPosition;

                if (player.Contract.Club.Name.Length == 0)
                    this.PlayerProfileClubTextBox.Text = "Free Player";
                else
                {
                    this.PlayerProfileClubTextBox.Text = player.Contract.Club.Name;
                    if (player.CoContract != null)
                        this.PlayerProfileClubTextBox.Text += Environment.NewLine + "/" + player.CoContract.Club.Name;

                    if (player.LoanContract != null)
                        this.PlayerProfileClubTextBox.Text += Environment.NewLine + "(" + player.LoanContract.Club.Name + ")";
                }

                this.PlayerProfileConsistencyTextBox.Text = ((int)(player.HiddenSkills.Consistency * 0.2 + 0.5)).ToString();
                this.PlayerProfileDirtynessTextBox.Text = ((int)(player.HiddenSkills.Dirtyness * 0.2 + 0.5)).ToString();
                this.PlayerProfileImportantMatchesTextBox.Text = ((int)(player.HiddenSkills.ImportantMatches * 0.2 + 0.5)).ToString();
                this.PlayerProfileInjuryPronenessTextBox.Text = ((int)(player.HiddenSkills.InjuryProness * 0.2 + 0.5)).ToString();
                this.PlayerProfileVersatilityTextBox.Text = ((int)(player.HiddenSkills.Versatility * 0.2 + 0.5)).ToString();
                if (!this.mainForm.playersPRID.Contains(player.ID))
                    this.mainForm.calculatePlayerPR(player); 
                PositionalRatings pr = (PositionalRatings)this.mainForm.playersPRID[player.ID];
                this.PlayerProfilePositionalRatingTextBox.Text = "Best as: " + pr.desc + " (" + (pr.bestPosR * 0.01f).ToString("P2") + ")";
                
                this.PlayerProfileAdaptabilityTextBox.Text = player.MentalTraitsSkills.Adaptability.ToString();
                this.PlayerProfileAmbitionTextBox.Text = player.MentalTraitsSkills.Ambition.ToString();
                this.PlayerProfileControversyTextBox.Text = player.MentalTraitsSkills.Controversy.ToString();
                this.PlayerProfileLoyaltyTextBox.Text = player.MentalTraitsSkills.Loyalty.ToString();
                this.PlayerProfilePressureTextBox.Text = player.MentalTraitsSkills.Pressure.ToString();
                this.PlayerProfileProfessionalismTextBox.Text = player.MentalTraitsSkills.Professionalism.ToString();
                this.PlayerProfileSportsmanshipTextBox.Text = player.MentalTraitsSkills.Sportsmanship.ToString();
                this.PlayerProfileTemperamentTextBox.Text = player.MentalTraitsSkills.Temperament.ToString(); 
                
                this.PlayerProfileLocalReputationTextBox.Text = player.CurrentPrestige.ToString();
                this.PlayerProfileNationalReputationTextBox.Text = player.NationalPrestige.ToString();
                this.PlayerProfileWorldReputationTextBox.Text = player.InternationalPrestige.ToString();
            }
            else if (staff!= null)
            {
                selectStaffInfo();

                // personal details
                this.StaffProfileIDTextBox.Text = staff.ID.ToString();
                this.StaffProfileFullNameTextBox.Text = staff.ToString();
                if (staff.Contract.Club.Name.Length == 0)
                    this.StaffProfileClubTextBox.Text = "Free Agent";
                else
                    this.StaffProfileClubTextBox.Text = staff.Contract.Club.Name;
                this.StaffProfileReputationTextBox.Text = "World: " + staff.InternationalPrestige 
                    + ", National:" + staff.NationalPrestige
                    + ", Local:" + staff.CurrentPrestige;
                if (StaffProfileClubTextBox.Text.Equals("Free Agent"))
                {
                    this.StaffProfileContractExpiryTextBox.Text = "No contract";
                    this.StaffProfileWageTextBox.Text = "No Wage";
                }
                else
                {
                    this.StaffProfileContractExpiryTextBox.Text = "Contract ends at " + staff.Contract.ContractExpiryDate.Date.ToShortDateString();
                    this.StaffProfileWageTextBox.Text = "Wage: " + (staff.Contract.WagePerWeek * this.mainForm.preferencesForm.WagesMultiplier * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat) + this.mainForm.preferencesForm.WagesString;
                }
                this.StaffProfileCATextBox.Text = staff.CurrentCoachingAbility + " CA";
                this.StaffProfilePATextBox.Text = staff.PotentialCoachingAbility + " PA";
                string staffRole = "";
                this.mainForm.find_staff_role(staff, ref staffRole, true);
                if (staffRole.Length == 0)
                    this.StaffProfileRoleTextBox.Text = "No Role";
                else
                    this.StaffProfileRoleTextBox.Text = staffRole;

                // tactical skills
                this.StaffProfileDepthTextBox.Text = ((int)(staff.TacticalSkills.Depth + 0.5)).ToString();
                this.StaffProfileDirectnessTextBox.Text = ((int)(staff.TacticalSkills.Directness + 0.5)).ToString();
                this.StaffProfileFlamboyancyTextBox.Text = ((int)(staff.TacticalSkills.Flamboyancy + 0.5)).ToString();
                this.StaffProfileFlexibilityTextBox.Text = ((int)(staff.TacticalSkills.Flexibility + 0.5)).ToString();
                this.StaffProfileFreeRolesTextBox.Text = ((int)(staff.TacticalSkills.FreeRoles + 0.5)).ToString();
                this.StaffProfileMarkingTextBox.Text = ((int)(staff.TacticalSkills.Marking + 0.5)).ToString();
                this.StaffProfileOffsideTextBox.Text = ((int)(staff.TacticalSkills.OffSide + 0.5)).ToString();
                this.StaffProfilePressingTextBox.Text = ((int)(staff.TacticalSkills.Pressing + 0.5)).ToString();
                this.StaffProfileSittingBackTextBox.Text = ((int)(staff.TacticalSkills.SittingBack + 0.5)).ToString();
                this.StaffProfileTempoTextBox.Text = ((int)(staff.TacticalSkills.Tempo + 0.5)).ToString();
                this.StaffProfileUseOfPlaymakerTextBox.Text = ((int)(staff.TacticalSkills.UseOfPlaymaker + 0.5)).ToString();
                this.StaffProfileUseOfSubstitutionsTextBox.Text = ((int)(staff.TacticalSkills.UseOfSubstitutions + 0.5)).ToString();
                this.StaffProfileWidthTextBox.Text = ((int)(staff.TacticalSkills.Width + 0.5)).ToString();

                // non tactical skills
                this.StaffProfileBuyingPlayersTextBox.Text = ((int)(staff.NonTacticalSkills.BuyingPlayers + 0.5)).ToString();
                this.StaffProfileHardnessOfTrainingTextBox.Text = ((int)(staff.NonTacticalSkills.HardnessOfTraining + 0.5)).ToString();
                this.StaffProfileMindGamesTextBox.Text = ((int)(staff.NonTacticalSkills.MindGames + 0.5)).ToString();
                this.StaffProfileSquadRotationTextBox.Text = ((int)(staff.NonTacticalSkills.SquadRotation + 0.5)).ToString();

                // chairman skills
                this.StaffProfileBusinessTextBox.Text = ((int)(staff.ChairmanSkills.Business + 0.5)).ToString();
                this.StaffProfileInterferenceTextBox.Text = ((int)(staff.ChairmanSkills.Interference + 0.5)).ToString();
                this.StaffProfilePatienceTextBox.Text = ((int)(staff.ChairmanSkills.Patience + 0.5)).ToString();
                this.StaffProfileResourcesTextBox.Text = ((int)(staff.ChairmanSkills.Resources + 0.5)).ToString();

                // coaching ratings
                if (!this.mainForm.staffCRID.Contains(staff.ID))
                    this.mainForm.calculateStaffCR(staff);
            }
            else if (team != null)
            {
                selectTeamInfo();

                // general details
                this.TeamProfileIDTextBox.Text = team.Club.ID.ToString();
                this.TeamProfileNameTextBox.Text = team.Club.Name;
                this.TeamProfileTrainingGroundTextBox.Text = team.Club.TrainingGround.ToString();
                this.TeamProfileYouthGroundTextBox.Text = team.Club.YouthGround.ToString();
                if (team.Club.YouthAcademy == 0)
                    this.TeamProfileYouthAcademyTextBox.Text = "No";
                else if (team.Club.YouthAcademy == 1)
                    this.TeamProfileYouthAcademyTextBox.Text = "Yes";
                else
                    this.TeamProfileYouthAcademyTextBox.Text = "Unavailable";
                //this.TeamProfileReputationTextBox.Text = team.Prestige.ToString();

                // finance details
                if (team.Club.Finances.RemainingTransferBudget != 27571580)
                {
                    this.TeamProfileTotalTransferTextBox.Text = (team.Club.Finances.SeasonTransferBudget * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
                    this.TeamProfileRemainingTransferTextBox.Text = (team.Club.Finances.RemainingTransferBudget * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
                    this.TeamProfileTotalWageTextBox.Text = (team.Club.Finances.WageBudget * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
                    this.TeamProfileUsedWageTextBox.Text = (team.Club.Finances.WageBudget * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
                }
                else
                {
                    this.TeamProfileTotalTransferTextBox.Text = "No budget";
                    this.TeamProfileRemainingTransferTextBox.Text = "No budget";
                    this.TeamProfileTotalWageTextBox.Text = "No budget";
                    this.TeamProfileUsedWageTextBox.Text = "No budget";
                }

                this.TeamProfileBalanceTextBox.Text = (team.Club.Finances.Balance * this.mainForm.preferencesForm.Currency).ToString("C0", this.mainForm.preferencesForm.CurrencyFormat);
            }
            setColors();
        }

        private void ClearToolStripButton_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void StaffProfileRatingsFitnessPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsFitnessPictureBox, this.StaffProfileIDTextBox.Text, e, 0);
        }

        private void StaffProfileRatingsGoalkeepersPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsGoalkeepersPictureBox, this.StaffProfileIDTextBox.Text, e, 1);
        }

        private void StaffProfileRatingsTacticsPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsTacticsPictureBox, this.StaffProfileIDTextBox.Text, e, 2);
        }

        private void StaffProfileRatingsBallControlPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsBallControlPictureBox, this.StaffProfileIDTextBox.Text, e, 3);
        }

        private void StaffProfileRatingsDefendingPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsDefendingPictureBox, this.StaffProfileIDTextBox.Text, e, 4);
        }

        private void StaffProfileRatingsAttackingPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsAttackingPictureBox, this.StaffProfileIDTextBox.Text, e, 5);
        }

        private void StaffProfileRatingsShootingPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsShootingPictureBox, this.StaffProfileIDTextBox.Text, e, 6);
        }

        private void StaffProfileRatingsSetPiecesPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.mainForm.StaffProfileRatingsPaint(ref StaffProfileRatingsSetPiecesPictureBox, this.StaffProfileIDTextBox.Text, e, 7);
        }

        private void OnTopToolStripButton_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            if (this.TopMost)
            {
                this.OnTopToolStripButton.Image = global::FMScout.Properties.Resources.OnTop;
                this.OnTopToolStripButton.Text = "On Top: Yes";
                this.OnTopToolStripButton.ToolTipText = "On Top: Yes";
            }
            else
            {
                this.OnTopToolStripButton.Image = global::FMScout.Properties.Resources.OnTopDisabled;
                this.OnTopToolStripButton.Text = "On Top: No";
                this.OnTopToolStripButton.ToolTipText = "On Top: No";
            }
        }
    }
}
