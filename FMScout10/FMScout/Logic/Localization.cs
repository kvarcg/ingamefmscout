using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FMScout
{
    public class ScoutLocalization
    {

        public ScoutLocalization()
        {
            for (int i = 0; i < currencies.Count(); ++i)
                currenciesNative.Add(currencies[i]);
            for (int i = 0; i < wages.Count(); ++i)
                wagesNative.Add(wages[i]);
            for (int i = 0; i < heights.Count(); ++i)
                heightsNative.Add(heights[i]);
            for (int i = 0; i < weights.Count(); ++i)
                weightsNative.Add(weights[i]);
            for (int i = 0; i < YesNo.Count(); ++i)
                editing.Add(YesNo[i]);
            for (int i = 0; i < regions.Count(); ++i)
                regionsNative.Add(regions[i]);
            for (int i = 0; i < playerColumns.Count(); ++i)
                playerNativeColumns.Add(playerColumns[i]);
            for (int i = 0; i < staffColumns.Count(); ++i)
                staffNativeColumns.Add(staffColumns[i]);
            for (int i = 0; i < teamColumns.Count(); ++i)
                teamNativeColumns.Add(teamColumns[i]);
            for (int i = 0; i < shortlistColumns.Count(); ++i)
                shortlistNativeColumns.Add(shortlistColumns[i]);
        }

        public ObservableCollection<String> currenciesNative = new ObservableCollection<String>();
        public ObservableCollection<String> wagesNative = new ObservableCollection<String>();
        public ObservableCollection<String> heightsNative = new ObservableCollection<String>();
        public ObservableCollection<String> weightsNative = new ObservableCollection<String>();
        public ObservableCollection<String> editing = new ObservableCollection<String>();
        public ObservableCollection<String> regionsNative = new ObservableCollection<String>();
        public ObservableCollection<String> playerNativeColumns = new ObservableCollection<String>();
        public ObservableCollection<String> staffNativeColumns = new ObservableCollection<String>();
        public ObservableCollection<String> teamNativeColumns = new ObservableCollection<String>();
        public ObservableCollection<String> shortlistNativeColumns = new ObservableCollection<String>();

        public ObservableCollection<String> currencies = new ObservableCollection<String>
        {
            "British Pound",
            "Euro",
            "US Dollars"
        };

        public ObservableCollection<String> wages = new ObservableCollection<String>
        {
            "Weekly",
            "Monthly",
            "Yearly"
        };

        public ObservableCollection<String> heights = new ObservableCollection<String>
        {
            "Centimeters",
            "Meters"
        };

        public ObservableCollection<String> weights = new ObservableCollection<String>
        {
            "Kilos",
            "Pounds"
        };

        public ObservableCollection<String> YesNo = new ObservableCollection<String>
        {
            "Yes",
            "No"
        };

        public ObservableCollection<String> YesNoEmpty = new ObservableCollection<String>
        {
            "",
            "Yes",
            "No"
        };

        public ObservableCollection<String> contractStatuses = new ObservableCollection<String>
        { 
           "",
           "Expiring (6 months)",
           "Expiring (1 year)",
           "Free Transfer"
        };

        public ObservableCollection<String> regions = new ObservableCollection<String>
        { 
            "",
            "African",
            "Asian",
            "European",
            "North American",
            "South American",
            "Oceanian"
        };

        public ObservableCollection<String> WindowGeneralLabels = new ObservableCollection<String>()
        { 
            "General Settings",
            "Currency",
            "Wage",
            "Distance/Height",
            "Weight",
            "Allow Editing",
            "Language",
            "Theme",
            "Wonderkids Max Age",
            "Wonderkids Min PA",
            "Wonderstaff Min PA",
            "Wonderteams Min Rep",
            "cm",
            "m",
            "kg",
            "lbs"
        };


        public static int WG_CM = 12;
        public static int WG_M = 13;
        public static int WG_KG = 14;
        public static int WG_LBS = 15;

        public ObservableCollection<String> WindowPreferencesLabels = new ObservableCollection<String>()
        {
            "Preferences",
            "Settings",
            "General",
            "Player Columns",
            "Staff Columns",
            "Team Columns",
            "Shortlist Columns",
            "Player Column Settings",
            "Staff Column Settings",
            "Team Column Settings",
            "Shortlist Column Settings",
            "Select All",
            "Clear",
            "Default",
            "Use Changes",
            "Cancel Changes",
            "Save custom settings if you wish to",
            "have them next time you use the scout or click here to remove them.",
            "Save current setting",
            "Delete current setting",
            "Set as default",
            "Use changes without saving",
            "Just close the window"
        };

        public ObservableCollection<String> WindowAboutLabels = new ObservableCollection<String>()
        {
           "FM Assistant10",
           "OK",
           "Real-Time scouting utility for FM 2010.",
           "Special thanks goes to DrBernhard at the SIGames forums for his work on the FM2010 Ingame/Scout Framework.",
           "Thanks to Stam for his help with the icons.",
           "Many thanks to all my beta testers.",
           "Localization Credits",
           "Italian: Giovanni",
           "Turkish: Ufuk Gültin",
           "Contact Details",
           "FMAssistant10 Homepage",
           "Email me",
           "Bug Report"
        };

        public ObservableCollection<String> WindowLoadingLabels = new ObservableCollection<String>()
        {
            "Looking FM 2010 10.3...Please wait",
            "Found FM 2010",
            "Loading Data",
            "Loading FM Data",
            "Generating Scout Data",
            "Loaded FM 2010",
            "in",
            "seconds",
            "Error Loading FM 2010 10.3",
            "Make sure you have a new or loaded game of FM 2010 10.3 up and running",
            "Closing in"
        };

        public ObservableCollection<String> WindowDonateLabels = new ObservableCollection<String>()
        {
            "FM Assistant10 Donation",
            "Yes, you rock!",
            "No, but you still rock!",
            "This tool is and always will be provided to you as a free utility to make your game more enjoyable.",
			"However, if you would like to encourage this project you can do it with a small donation or with a simple thanks :)",
			"May the FM force be with you, Kostas."
        };

        public ObservableCollection<String> WindowInfoLabels = new ObservableCollection<String>()
        {
            "Current Settings",
            "Game Settings",
            "Game Status",
            "Loaded",
            "Game Version",
            "Players",
            "Staff",
            "Teams",
            "Scout Settings",
            "Settings",
            "Language",
            "Theme",
            "Currency",
            "Wage",
            "Height",
            "Weight",
            "Editor Settings",
            "Editing Settings"
        };

        public ObservableCollection<String> WindowCustomizeColumnsLabels = new ObservableCollection<String>()
        {
            "Customize Columns"
        };

        public ObservableCollection<String> MenuLabels = new ObservableCollection<String>()
        {
            "Load",
            "Exit",
            "Shortlist",
            "Import Shortlist",
            "Export Shortlist",
            "Export Selected Shortlist",
            "Add To Shortlist",
            "Search",
            "Players View",
            "Staff View",
            "Teams View",
            "Shortlist View",
            "Search Now",
            "Clear",
            "Clear Player Fields",
            "Clear Staff Fields",
            "Clear Team Fields",
            "Clear Shortlist",
            "Clear All",
            "Tools",
            "Preferences",
            "Help",
            "About",
            "Donate"
        };

        public ObservableCollection<String> WindowMainLabels = new ObservableCollection<String>()
        {
            "Players",
            "Staff",
            "Teams",
            "Shortlist",
            "General",
            "Attributes",
            "Current Game Date",
            "Current Game Screen",
            "Show Info",
            "Hide Info"
        };

        public ObservableCollection<String> GeneralSearchLabels = new ObservableCollection<String>()
        {
            "Search",
            "Results",
            "customize columns"
        };

        public ObservableCollection<String> PlayerSearchLabels = new ObservableCollection<String>()
        {
            "Full Name",
            "name",
            "Nation",
            "nation",
            "Club",
            "club",
            "Region",
            "Age",
            "CA",
            "PA",
            "PR%",
            "Best PR",
            "Wage",
            "Value",
            "Sale Value",
            "Contract Status",
            "Ownership",
            "EU",
            "Regen",
            "Pref Foot",
            "Sides",
            "Positions",
            "GK",
            "SW",
            "D",
            "WB",
            "DM",
            "M",
            "AM",
            "ST",
            "L",
            "R",
            "C",
            "Free",
            "F"
        };

        public static int L_CA = 8;
        public static int L_PA = 9;
        public static int L_GK = 22;
        public static int L_SW = 23;
        public static int L_D = 24;
        public static int L_WB = 25;
        public static int L_DM = 26;
        public static int L_M = 27;
        public static int L_AM = 28;
        public static int L_ST = 29;
        public static int L_L = 30;
        public static int L_R = 31;
        public static int L_C = 32;
        public static int L_FREE = 33;
        public static int L_F = 34;

        public ObservableCollection<String> StaffSearchLabels = new ObservableCollection<String>()
        {
           "Full Name",
           "name",
            "Nation",
            "nation",
            "Club",
            "club",
            "Role",
            "Region",
            "Age",
            "CA",
            "PA",
            "Contract Status",
            "Best CR",
            "Regen"
        };

        public ObservableCollection<String> TeamSearchLabels = new ObservableCollection<String>()
        {
            "Name",
            "name",
            "Nation",
            "nation",
            "Stadium",
            "stadium",
            "Team Type",
            "Reputation",
            "Region",
            "Transfer Budget",
            "Wage Budget"
        };

        public ObservableCollection<String> PlayerProfileLabels = new ObservableCollection<String>()
        {
            "View Technical",
            "View Goalkeeping",
            "Positional Rating:",
            "Best as:",
            "Team Squad",
            "Contract Started",
            "Contract Expiring",
            "Basic Wage",
            "Appearance Bonus",
            "Goal Bonus",
            "Clean Sheet Bonus",
            "Condition",
            "Fitness",
            "Morale",
            "Jadedness",
            "Happiness",
            "Squad No",
            "Left Foot",
            "Right Foot"
        };

        public ObservableCollection<String> StaffProfileLabels = new ObservableCollection<String>() 
        { 
            "Contract Started",
            "Contract Expiring",
            "Basic Wage",
        };

        public ObservableCollection<String> SearchingResults = new ObservableCollection<String>()
        {
            "Search",
            "Searching",
            "Stop",
            "player entries found",
            "wonderkid entries found",
            "staff entries found",
            "wonderstaff entries found",
            "team entries found",
            "wonderteam entries found",
            "shortlist entries",
            "Query Took",
            "sec",
            "Calculated PR for",
            "Calculated CR for",
            "players",
            "staff",
            "teams",
            "Wonderkids",
            "WonderStaff",
            "WonderTeams",
            "Free Player",
            "Free Agent"
        };
        public static int SR_SEARCH = 0;
        public static int SR_SEARCHING = 1;
        public static int SR_STOP = 2;
        public static int SR_PLAYERENTRIESFOUND = 3;
        public static int SR_WONDERKIDENTRIESFOUND = 4;
        public static int SR_STAFFENTRIESFOUND = 5;
        public static int SR_WONDERSTAFFENTRIESFOUND = 6;
        public static int SR_TEAMENTRIESFOUND = 7;
        public static int SR_WONDERTEAMENTRIESFOUND = 8;
        public static int SR_SHORTLISTENTRIESFOUND = 9;
        public static int SR_QUERYTOOK = 10;
        public static int SR_SEC = 11;
        public static int SR_CALCPRFOR = 12;
        public static int SR_CALCCRFOR = 13;
        public static int SR_PLAYERS = 14;
        public static int SR_STAFF = 15;
        public static int SR_TEAMS = 16;
        public static int SR_WONDERKIDS = 17;
        public static int SR_WONDERSTAFF = 18;
        public static int SR_WONDERTEAMS = 19;
        public static int SR_FREEPLAYER = 20;
        public static int SR_FREEAGENT = 21;

        public ObservableCollection<String> ProfileGenericLabels = new ObservableCollection<String>()
        {
            "None",
            "Unavailable",
            "Unknown",
            "No",
            "Yes",
            "Add To Shortlist",
            "Remove From Shortlist",
            "All Squads",
            "Team",
            "Uncapped",
            "years old",
            "Not HG",
            "Formed At:",
            "EU Member:",
            "No Sale Value",
            "Both Feet",
            "Left Foot",
            "Right Foot",
            "No Role",
            "No Contract",
            "No Wage",
            "No Bonus",
            "caps",
            "goals",
            "Not A Regen",
            "Is Regen",
            "Nation Players",
            "List Players",
            "List Staff",
            "Heal Team",
            "No Budget",
            "No Owner",
            "No Location",
            "No Nearby Stadium",
            "Empty",
            "Att",
            "Playmaker",
            "Free",
            "Role",
            "Club HG",
            "Country HG"
        };
        public static int PG_NONE=0;
        public static int PG_UNAVAILABLE=1;
        public static int PG_UNKNOWN=2;
        public static int PG_NO=3;
        public static int PG_YES = 4;
        public static int PG_ADDTOSHORTLIST = 5;
        public static int PG_REMOVEFROMSHORTLIST=6;
        public static int PG_ALLSQUADS=7;
        public static int PG_TEAM=8;
        public static int PG_UNCAPPED=9;
        public static int PG_YEARSOLD=10;
        public static int PG_NOTHG=11;
        public static int PG_FORMEDAT=12;
        public static int PG_EUMEMBER=13;
        public static int PG_NOSALEVALUE=14;
        public static int PG_BOTHFEET=15;
        public static int PG_LEFTFOOT=16;
        public static int PG_RIGHTFOOT=17;
        public static int PG_NOROLE=18;
        public static int PG_NOCONTRACT=19;
        public static int PG_NOWAGE=20;
        public static int PG_NOBONUS=21;
        public static int PG_CAPS=22;
        public static int PG_GOALS=23;
        public static int PG_NOTAREGEN=24;
        public static int PG_ISREGEN=25;
        public static int PG_NATIONPLAYERS=26;
        public static int PG_LISTPLAYERS=27;
        public static int PG_LISTSTAFF=28;
        public static int PG_HEALTEAM=29;
        public static int PG_NOBUDGET=30;
        public static int PG_NOOWNER=31;
        public static int PG_NOLOCATION=32;
        public static int PG_NONEARBYSTADIUM=33;
        public static int PG_EMPTY = 34;
        public static int PG_ATT = 35;
        public static int PG_PLAYMAKER = 36;
        public static int PG_FREE = 37;
        public static int PG_ROLE = 38;
        public static int PG_CLUBHG = 39;
        public static int PG_COUNTRYHG = 40;

        public ObservableCollection<String> TeamProfileLabels = new ObservableCollection<String>()
        {
            "Year Founded",
            "National",
            "Status",
            "Max Affiliated Clubs",
            "Affiliated Clubs",
            "Training Facilities",
            "Youth Facilities",
            "Youth Academy",
            "Max Attendance",
            "Avg Attendance",
            "Min Attendance",
            "Reputation",
            "Total Transfer Budget",
            "Remaining Transfer Budget",
            "Balance",
            "Total Wage Budget",
            "Used Wage Budget",
            "Revenue Available",
            "Decay",
            "Field Width",
            "Field Length",
            "Current Capacity",
            "Seating Capacity",
            "Expansion Capacity",
            "Used Capacity"
        };

        public ObservableCollection<String> WindowProfileLabels = new ObservableCollection<String>()
        {
            "Player Profiles",
            "Staff Profiles",
            "Team Profiles",
            "Players List",
            "Staff List",
            "Teams List"
        };

        #region players

        public ObservableCollection<String> playerSearchAttributesGroupBoxes = new ObservableCollection<String>
        {
            "Technical Attributes",
            "Physical Attributes",
            "Mental Attributes",
            "Hidden Attributes",
            "Goalkeeping Attributes",
            "Mental Traits Attributes",
            "Personal Details",
            "Contract Details",
            "Other Attributes"
        };

        public ObservableCollection<String> prefFoots = new ObservableCollection<String>
        { 
           "Both",
           "Right",
           "Left"
        };

        public ObservableCollection<String> bestprs = new ObservableCollection<String>
        { 
           "",
           "GK",
           "DC",
           "DL",
           "DR",
           "DMC",
           "DML",
           "DMR",
           "AMC",
           "AML",
           "AMR",
           "Fast FC",
           "Target FC",
        };
        public static int BP_GK = 1;
        public static int BP_DC = 2;
        public static int BP_DL = 3;
        public static int BP_DR = 4;
        public static int BP_DMC = 5;
        public static int BP_DML = 6;
        public static int BP_DMR = 7;
        public static int BP_AMC = 8;
        public static int BP_AML = 9;
        public static int BP_AMR = 10;
        public static int BP_FASTFC = 11;
        public static int BP_TARGETFC = 12;

        public ObservableCollection<String> ownerShips = new ObservableCollection<String>
        { 
           "",
           "Loan",
           "Co-Contract"
        };

        #endregion

        #region staff

        public ObservableCollection<String> staffSearchAttributesGroupBoxes = new ObservableCollection<String>
        {
            "Tactical Attributes",
            "Mental Attributes",
            "Coaching Attributes",
            "Non Tactical Attributes",
            "Chairman Attributes",
            "Personal Details",
            "Contract Details",
			"Best Ratings"
        };

        public ObservableCollection<String> staffDisplayRatings = new ObservableCollection<String>()
        { 
            "Fit", 
            "GK", 
            "Tac", 
            "BC", 
            "Def", 
            "Att", 
            "Shoot", 
            "Set" 
        };

        public static int SD_FIT = 0;
        public static int SD_GK = 1;
        public static int SD_TAC = 2;
        public static int SD_BC = 3;
        public static int SD_DEF = 4;
        public static int SD_ATT = 5;
        public static int SD_SHOOT = 6;
        public static int SD_SET = 7;

        public ObservableCollection<String> staffRoles = new ObservableCollection<String>
        { 
            "",
            "Chairman",
            "Director",
            "Manager",
            "Assistant Manager",
            "1st Team Coach",
            "Youth Coach",
            "Coach",
            "1st/Youth/Coach",
            "Fitness Coach",
            "Goalkeeping Coach",
            "Physio",
            "Scout"
        };

        public static int R_CHAIRMAN = 0;
        public static int R_DIRECTOR = 1;
        public static int R_MANAGER = 2;
        public static int R_ASSISTANTMANAGER = 3;
        public static int R_1STTEAMCOACH = 4;
        public static int R_YOUTHCOACH = 5;
        public static int R_COACH = 6;
        public static int R_1STYOUTHCOACH = 7;
        public static int R_FITNESSCOACH = 8;
        public static int R_GOAlKEEPINGCOACH = 9;
        public static int R_PHYSIO = 10;
        public static int R_SCOUT = 11;

        public ObservableCollection<String> bestcrs = new ObservableCollection<String>
        { 
            "",
            "Fitness",
            "Goalkeepers",
            "Tactics",
            "Ball Control",
            "Defending",
            "Attacking",
            "Shooting",
            "Set Pieces"
        };

        #endregion

        #region teams

        public ObservableCollection<String> teamGroupBoxes = new ObservableCollection<String>
        {
            "General",
            "Finance Details",
            "Stadium Details"
        };

        public ObservableCollection<String> teamtypes = new ObservableCollection<String>
        { 
            "",
            "National",
            "Club"
        };

        public ObservableCollection<String> reputations = new ObservableCollection<String>
        { 
            "",
            "World",
            "National",
            "Local",
            "Unknown"
        };

        #endregion

        public static int SR_PLAYERCOLUMNSCORNER = 21;
        public ObservableCollection<String> playerColumns = new ObservableCollection<String>{
            "S",
            "ID",
            "Full Name",
            "Nation",
            "Club",
            "Team Squad",
            "Position",
            "Age",
            "CA",
            "PA",
            "ADiff",
            "Best PR",
            "Best PR%",
            "Current Value",
            "Sale Value",
            "Contract Started",
            "Contract Expiring",
            "Current Wage",
            "World Reputation",
            "National Reputation",
            "Local Reputation",
            "Corners",
            "Crossing",
            "Dribbling",
            "Finishing",
            "First Touch",
            "Free Kicks",
            "Heading",
            "Long Shots",
            "Long Throws",
            "Marking",
            "Passing",
            "Penalty Taking",
            "Tackling",
            "Technique",
            "Acceleration",
            "Agility",
            "Balance",
            "Jumping",
            "Natural Fitness",
            "Pace",
            "Stamina",
            "Strength",
            "Left Foot",
            "Right Foot",
            "Aggression",
            "Anticipation",
            "Bravery",
            "Composure",
            "Concentration",
            "Creativity",
            "Decisions",
            "Determination",
            "Flair",
            "Influence",
            "Off The Ball",
            "Positioning",
            "Team Work",
            "WorkRate",
            "Consistency",
            "Dirtyness",
            "Important Matches",
            "Injury Proneness",
            "Versatility",
            "Aerial Ability",
            "Command Of Area",
            "Communication",
            "Eccentricity",
            "Handling",
            "Kicking",
            "One On Ones",
            "Reflexes",
            "Rushing Out",
            "Tendency To Punch",
            "Throwing",
            "Adaptability",
            "Ambition",
            "Controversy",
            "Loyalty",
            "Pressure",
            "Professionalism",
            "Sportsmanship",
            "Temperament"
        };

        public static int SR_STAFFCOLUMNDEPTH = 17;
        public ObservableCollection<String> staffColumns = new ObservableCollection<String>{
			"ID",
            "Full Name",
            "Nation",
            "Club",
            "Role",
            "Age",
            "CA",
            "PA",
            "ADiff",
            "Best CR",
            "Best CR Stars",
            "Contract Started",
            "Contract Expiring",
            "Current Wage",
            "World Reputation",
            "National Reputation",
            "Local Reputation",
            "Depth",
            "Directness",
            "Flamboyancy",
            "Flexibility",
            "Free Roles",
            "Marking",
            "Offside",
            "Pressing",
            "Sitting Back",
            "Tempo",
            "Use Of Playmaker",
            "Use Of Substitutions",
            "Width",
            "Adaptability",
            "Ambition",
            "Controversy",
            "Determination",
            "Loyalty",
            "Pressure",
            "Professionalism",
            "Sportsmanship",
            //"Temperament",
            "Judging Player Ability",
            "Judging Player Potential",
            "Level Of Discipline",
            "Motivating",
            "Physiotherapy",
            "Tactical Knowledge",
            "Attacking",
            "Defending",
            "Fitness",
            "Goalkeepers",
            "Mental",
            "Player",
            "Tactical",
            "Technical",
            "Man Management",
            "Working With Youngsters",
            "Buying Players",
            "Hardness Of Training",
            "Mind Games",
            "Squad Rotation",
            "Business",
            "Interference",
            "Patience",
            "Resources"
		};

        public ObservableCollection<String> teamColumns = new ObservableCollection<String>{
			"ID",
            "Name",
            "Nation",
            "Reputation",
            "Status",
            "Stadium",
            "Transfer Budget",
            "Remaining Budget",
            "Wage Budget",
            "Wage Used",
            "Budget Balance",
            "Transfer Revenue Available",
            "Current Affiliated Clubs",
            "Max Affiliated Clubs",
            "Training Facilities",
            "Youth Facilities",
            "Maximum Attendance",
            "Average Attendance",
            "Minimum Attendance",
            "Decay",
            "Field Width",
            "Field Length",
            "Current Capacity ",
            "Seating Capacity",
            "Expansion Capacity",
            "Used Capacity"
		};

        public ObservableCollection<String> shortlistColumns = new ObservableCollection<String>{
            "ID",
            "Full Name",
            "Nation",
            "Club",
            "Team Squad",
            "Position",
            "Age",
            "CA",
            "PA",
            "ADiff",
            "Best PR",
            "Best PR%",
            "Current Value",
            "Sale Value",
            "Contract Started",
            "Contract Expiring",
            "Current Wage",
            "World Reputation",
            "National Reputation",
            "Local Reputation",
            "Corners",
            "Crossing",
            "Dribbling",
            "Finishing",
            "First Touch",
            "Free Kicks",
            "Heading",
            "Long Shots",
            "Long Throws",
            "Marking",
            "Passing",
            "Penalty Taking",
            "Tackling",
            "Technique",
            "Acceleration",
            "Agility",
            "Balance",
            "Jumping",
            "Natural Fitness",
            "Pace",
            "Stamina",
            "Strength",
            "Left Foot",
            "Right Foot",
            "Aggression",
            "Anticipation",
            "Bravery",
            "Composure",
            "Concentration",
            "Creativity",
            "Decisions",
            "Determination",
            "Flair",
            "Influence",
            "Off The Ball",
            "Positioning",
            "Team Work",
            "WorkRate",
            "Consistency",
            "Dirtyness",
            "Important Matches",
            "Injury Proneness",
            "Versatility",
            "Aerial Ability",
            "Command Of Area",
            "Communication",
            "Eccentricity",
            "Handling",
            "Kicking",
            "One On Ones",
            "Reflexes",
            "Rushing Out",
            "Tendency To Punch",
            "Throwing",
            "Adaptability",
            "Ambition",
            "Controversy",
            "Loyalty",
            "Pressure",
            "Professionalism",
            "Sportsmanship",
            "Temperament"
        };
    }
}