using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using Young3.FMSearch.Interface;
using Young3.FMSearch.Core.Entities.InGame;
using Young3.FMSearch.Core.Managers;
using Young3.FMSearch.Core.Offsets;


namespace FMScout
{
    public static class ScoutContext
    {
        static Context fmContext = null;

        public static Context getScoutContext()
        {
            if (fmContext == null)
            {
                fmContext = new Context();
            }
            return fmContext;
        }
    }

    public struct PositionalRatings
    {
        public float FCTarget;
        public float FCFast;
        public float AMR;
        public float AML;
        public float AMC;
        public float DMR;
        public float DML;
        public float DMC;
        public float DR;
        public float DL;
        public float DC;
        public float GK;
        public float bestPosR;
        public string bestPos;
    }

    public struct CoachingRatings
    {
        public int Fitness;
        public int Goalkeepers;
        public int BallControl;
        public int Tactics;
        public int Defending;
        public int Attacking;
        public int Shooting;
        public int SetPieces;
        public string BestCR;
        public int BestCRStars;
    }

    public enum PLAYERCLUBSTATE
    {
        PCS_FREE,
        PCS_LOAN,
        PCS_COOWN,
        PCS_DEFAULT
    };

    public enum STAFFCLUBSTATE
    {
        SCS_FREE,
        SCS_NATIONAL,
        SCS_DEFAULT
    };

    public enum PLAYEREUSTATE
    {
        PES_EU,
        PES_NONEU
    };

    public enum TEAMSTATE
    {
        TS_NATIONAL,
        TS_NONNATIONAL
    };


    public class Context
    {
        public FMContext fm = null;
        private GlobalFuncs globalFuncs = null;
        public bool scoutLoaded = false;
        public bool loaded = false;
        public int playersPRTotal = 0;
        public int staffCRTotal = 0;
        public Hashtable playersPRID = null;
        public Hashtable staffCRID = null;
        public Hashtable shortlistIDList = null;

        public Context()
        {
            shortlistIDList = new Hashtable();
            globalFuncs = Globals.getGlobalFuncs();
        }

        public void init()
        {
            if (fm != null) fm.Dispose();
            fm = new FMContext(Young3.FMSearch.Core.Entities.DatabaseModeEnum.Cached);

            if (fm.CheckProcessAndGame())
            {
                loaded = true;
            }
            else
            {
                loaded = false;
            } 
        }

        public void loadFMData()
        {
           fm.LoadDataForCheckedGame(false);
        }

        public void loadScoutData()
        {
            int i = -1;
            if (globalFuncs.EUcountries == null)
            {
                globalFuncs.EUcountries = new Hashtable();
                globalFuncs.EUcountries.Add("Austria", ++i);
                globalFuncs.EUcountries.Add("Belgium", ++i);
                globalFuncs.EUcountries.Add("Bulgaria", ++i);
                globalFuncs.EUcountries.Add("Cyprus", ++i);
                globalFuncs.EUcountries.Add("Czech Republic", ++i);
                globalFuncs.EUcountries.Add("Denmark", ++i);
                globalFuncs.EUcountries.Add("England", ++i);
                globalFuncs.EUcountries.Add("Estonia", ++i);
                globalFuncs.EUcountries.Add("Finland", ++i);
                globalFuncs.EUcountries.Add("France", ++i);
                globalFuncs.EUcountries.Add("Germany", ++i);
                globalFuncs.EUcountries.Add("Greece", ++i);
                globalFuncs.EUcountries.Add("Hungary", ++i);
                globalFuncs.EUcountries.Add("Ireland", ++i);
                globalFuncs.EUcountries.Add("Italy", ++i);
                globalFuncs.EUcountries.Add("Latvia", ++i);
                globalFuncs.EUcountries.Add("Lithuania", ++i);
                globalFuncs.EUcountries.Add("Luxembourg", ++i);
                globalFuncs.EUcountries.Add("Malta", ++i);
                globalFuncs.EUcountries.Add("Holland", ++i);
                globalFuncs.EUcountries.Add("Poland", ++i);
                globalFuncs.EUcountries.Add("Portugal", ++i);
                globalFuncs.EUcountries.Add("Romania", ++i);
                globalFuncs.EUcountries.Add("Slovakia", ++i);
                globalFuncs.EUcountries.Add("Slovenia", ++i);
                globalFuncs.EUcountries.Add("Spain", ++i);
                globalFuncs.EUcountries.Add("Sweden", ++i);
            }

            if (globalFuncs.allClubs == null)
                globalFuncs.allClubs = new Hashtable();
            else
                globalFuncs.allClubs.Clear();

            if (globalFuncs.players == null)
                globalFuncs.players = new ObservableCollection<String>();
            else
                globalFuncs.players.Clear();

            if (globalFuncs.playersFixed == null)
                globalFuncs.playersFixed = new ObservableCollection<String>();
            else
                globalFuncs.playersFixed.Clear();

            if (globalFuncs.staff == null)
                globalFuncs.staff = new ObservableCollection<String>();
            else
                globalFuncs.staff.Clear();

            if (globalFuncs.staffFixed == null)
                globalFuncs.staffFixed = new ObservableCollection<String>();
            else
                globalFuncs.staffFixed.Clear();

            if (globalFuncs.clubs == null)
                globalFuncs.clubs = new ObservableCollection<String>();
            else
                globalFuncs.clubs.Clear();

            if (globalFuncs.countries == null)
                globalFuncs.countries = new ObservableCollection<String>();
            else
                globalFuncs.countries.Clear();

            if (globalFuncs.stadiums == null)
                globalFuncs.stadiums = new ObservableCollection<String>();
            else
                globalFuncs.stadiums.Clear();

            if (!globalFuncs.isDebug && loaded)
            {
                foreach (Team team in fm.Teams)
                {
                    if (team.Club != null)
                    {
                        if (!globalFuncs.allClubs.Contains(team.Club.ID))
                        {
                            globalFuncs.allClubs.Add(team.Club.ID, team.Club);
                            globalFuncs.clubs.Add(team.Club.Name);
                        }
                    }
                }

                string name = "";
                string nickname = "";
                foreach (Player player in fm.Players)
                {
                    if (player.FirstName.Length == 0) continue;

                    if (player.Age < 13 || player.Age > 90 || player.Nationality == null)
                        continue;

                    name = player.FirstName + " " + player.LastName;
                    nickname = player.Nickname;

                    if (!nickname.Equals(String.Empty))
                        name = nickname;

                    globalFuncs.players.Add(name);

                    name = name.ToLower();
                    globalFuncs.specialCharactersReplacement(ref name);
                    globalFuncs.playersFixed.Add(name);
                }

                foreach (Staff staff in fm.Staff)
                {
                    if (staff.FirstName.Length == 0) continue;

                    name = staff.FirstName + " " + staff.LastName;
                    nickname = staff.Nickname;

                    if (!nickname.Equals(String.Empty))
                        name = nickname;

                    globalFuncs.staff.Add(name);

                    name = name.ToLower();
                    globalFuncs.specialCharactersReplacement(ref name);
                    globalFuncs.staffFixed.Add(name);
                }

                foreach (Country country in fm.Countries)
                {
                    if (country.Name.Length == 0) continue;
                    globalFuncs.countries.Add(country.Name);
                }

                foreach (Stadium stadium in fm.Stadiums)
                {
                    if (stadium.Name.Length == 0) continue;
                    globalFuncs.stadiums.Add(stadium.Name);
                }

                if (playersPRID == null)
                    playersPRID = new Hashtable(fm.Players.Count());
                else
                    playersPRID.Clear();

                if (staffCRID == null)
                    staffCRID = new Hashtable(fm.Staff.Count());
                else
                    staffCRID.Clear();
            }
        }

        public void findPlayerContractQuery(Player player, ref Contract contract, ref String playerClub, ref PLAYERCLUBSTATE playerClubState)
        {
            contract = null;
            if (player.Contract != null)
            {
                if (player.ContractSecond != null)
                {
                    if (player.ContractSecond.ContractTypeSecond == ContractTypeSecond.Loan &&
                        player.ContractSecond.Club != null)
                    {
                        playerClub = player.ContractSecond.Club.Name + "/" + player.Contract.Club.Name;
                        playerClubState = PLAYERCLUBSTATE.PCS_LOAN;
                        contract = player.ContractSecond;
                    }
                    else if (player.ContractSecond.ContractTypeSecond == ContractTypeSecond.Coowned &&
                        player.ContractSecond.Club != null)
                    {
                        playerClub = player.Contract.Club.Name + "/" + player.ContractSecond.Club.Name;
                        playerClubState = PLAYERCLUBSTATE.PCS_COOWN;
                        contract = player.ContractSecond;
                    }
                }

                if (contract == null && player.Contract.Club != null)
                {
                    playerClub = player.Contract.Club.Name;
                    contract = player.Contract;
                    playerClubState = PLAYERCLUBSTATE.PCS_DEFAULT;
                }
            }
        }

        public void findPlayerContractGrid(Player player, ref Contract contract, ref int playerWage)
        {
            contract = null; 
            if (player.Contract != null)
            {
                if (player.ContractSecond != null)
                {
                    if (player.ContractSecond.ContractTypeSecond == ContractTypeSecond.Loan &&
                        player.ContractSecond.Club != null)
                    {
                        playerWage = player.ContractSecond.WagePerWeek;
                        contract = player.ContractSecond;
                    }
                    else if (player.ContractSecond.ContractTypeSecond == ContractTypeSecond.Coowned &&
                        player.ContractSecond.Club != null)
                    {
                        if (player.Contract.Club.Name.Equals(player.Team.Club.Name))
                            playerWage = player.Contract.WagePerWeek;
                        else
                        {
                            playerWage = player.ContractSecond.WagePerWeek;
                            contract = player.ContractSecond;
                        }
                    }
                }
                if (contract == null)
                {
                    playerWage = player.Contract.WagePerWeek;
                    contract = player.Contract;
                }
            }
        }

        public bool find_player_position(Player player, ref string pos, ref List<string> positions, ref List<string> sides, bool return_after_pos)
        {
            ObservableCollection<String> PlayerSearchLabels = globalFuncs.localization.PlayerSearchLabels;
                
            PositionalSkills pos_skills = player.PositionalSkills;
            sbyte comp = (sbyte)(15);
            bool is_goalkeeper = false;
            bool is_sweeper = false;
            bool is_wingback = false;
            bool is_defender = false;
            bool is_defensive_midfielder = false;
            bool is_midfielder = false;
            bool is_attacking_midfielder = false;
            bool is_forward = false;
            bool is_right = false;
            bool is_left = false;
            bool is_centre = false;
            bool is_free_role = false;

            if (pos_skills.Goalkeeper > comp)
            {
                is_goalkeeper = true;
                pos += PlayerSearchLabels[ScoutLocalization.L_GK];
            }
            if (pos_skills.Sweeper > comp)
            {
                is_sweeper = true;
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_SW];
            }
            if (pos_skills.WingBack > comp)
            {
                is_wingback = true;
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_WB];
            }
            if (pos_skills.Defender > comp)
            {
                is_defender = true;
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_D];
            }
            if (pos_skills.DefensiveMidfielder > comp)
            {
                is_defensive_midfielder = true;
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_DM];
            }
            if (pos_skills.Midfielder > comp)
            {
                is_midfielder = true;
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_M];
            }
            if (pos_skills.AttackingMidfielder > comp)
            {
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_AM];
                is_attacking_midfielder = true;
            }
            if (pos_skills.Attacker > comp)
            {
                if (pos.Length != 0) pos += ",";
                pos += PlayerSearchLabels[ScoutLocalization.L_F];
                is_forward = true;
            }

            pos += " ";

            if (pos_skills.Right > comp)
            {
                pos += PlayerSearchLabels[ScoutLocalization.L_R];
                is_right = true;
            }
            if (pos_skills.Left > comp)
            {
                pos += PlayerSearchLabels[ScoutLocalization.L_L];
                is_left = true;
            }
            if (pos_skills.Centre > comp)
            {
                pos += PlayerSearchLabels[ScoutLocalization.L_C];
                is_centre = true;
            }
            if (pos_skills.FreeRole > comp)
            {
                pos += "\\" + globalFuncs.localization.ProfileGenericLabels[ScoutLocalization.PG_FREE] + " " +
                              globalFuncs.localization.ProfileGenericLabels[ScoutLocalization.PG_ROLE];
                is_free_role = true;
            }

            if (return_after_pos) return true;

            bool found = false;
            foreach (string player_pos in positions)
            {
                found = false;
                if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_GK]) && (is_goalkeeper)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_SW]) && (is_sweeper)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_D]) && (is_defender)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_WB]) && (is_wingback)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_DM]) && (is_defensive_midfielder)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_M]) && (is_midfielder)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_AM]) && (is_attacking_midfielder)) found = true;
                else if (player_pos.Equals(PlayerSearchLabels[ScoutLocalization.L_ST]) && (is_forward)) found = true;
                if (found == false) return false;
            }
            
            foreach (string player_side in sides)
            {
                found = false;
                if (player_side.Equals(PlayerSearchLabels[ScoutLocalization.L_R]) && (is_right)) found = true;
                else if (player_side.Equals(PlayerSearchLabels[ScoutLocalization.L_L]) && (is_left)) found = true;
                else if (player_side.Equals(PlayerSearchLabels[ScoutLocalization.L_C]) && (is_centre)) found = true;
                else if (player_side.Equals(PlayerSearchLabels[ScoutLocalization.L_FREE]) && (is_free_role)) found = true;
                if (found == false) return false;
            }

            return true;
        }   

        public void find_staff_club(Staff staff, ref String staffClub, ref STAFFCLUBSTATE staffClubState)
        {
            if (staff.NationalTeam != null)
            {
                if (staff.NationalTeam.Club != null)
                {
                    if (globalFuncs.localization.regionsNative.Contains(staff.NationalTeam.Club.Country.Name))
                    {
                        staffClub = staff.NationalTeam.Club.Name;
                        staffClubState = STAFFCLUBSTATE.SCS_NATIONAL;
                    }
                }
            }
            if (staff.Team != null)
            {
                if (staff.Team.Club != null)
                {
                    if (/*staff.Team.Club.Chairman.ID != staff.ID && */staff.Contract != null)
                    {
                        if (staff.Contract.Club != null)
                            staffClub = staff.Contract.Club.Name;
                        else
                            staffClub = staff.Team.Club.Name;
                    }
                    else
                        staffClub = staff.Team.Club.Name;
                    staffClubState = STAFFCLUBSTATE.SCS_DEFAULT;
                }
            }

            if (staffClub.Length == 0)
            {
                staffClub += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEAGENT];
            }
        }

        public bool find_staff_role(Staff staff, ref string staff_role, ref string selectedItem, bool return_after_role)
        {
            ObservableCollection<String> staffRoles = globalFuncs.localization.staffRoles;
            
            if (staff.Contract != null)
            {
                int job = staff.Contract.JobType;
                if (job == 16)
                    staff_role = staffRoles[ScoutLocalization.R_MANAGER];
                else if (job == 20)
                    staff_role = staffRoles[ScoutLocalization.R_ASSISTANTMANAGER];
                else if (job == 54)
                    staff_role = staffRoles[ScoutLocalization.R_1STTEAMCOACH];
                else if (job == 48)
                    staff_role = staffRoles[ScoutLocalization.R_YOUTHCOACH];
                else if (job == 2)
                    staff_role = staffRoles[ScoutLocalization.R_COACH];
                else if (job == 26)
                    staff_role = staffRoles[ScoutLocalization.R_FITNESSCOACH];
                else if (job == 34)
                    staff_role = staffRoles[ScoutLocalization.R_GOAlKEEPINGCOACH];
                else if (job == 12)
                    staff_role = staffRoles[ScoutLocalization.R_PHYSIO];
                else if (job == 14)
                    staff_role = staffRoles[ScoutLocalization.R_SCOUT];

                if (return_after_role) return true;

                if (selectedItem.Equals(staffRoles[ScoutLocalization.R_1STYOUTHCOACH]))
                {
                    if (staff_role.Equals(staffRoles[ScoutLocalization.R_1STTEAMCOACH]) ||
                        staff_role.Equals(staffRoles[ScoutLocalization.R_YOUTHCOACH]) ||
                        staff_role.Equals(staffRoles[ScoutLocalization.R_COACH]))
                        return true;
                }
                else if (selectedItem.Equals(staff_role)) return true;
                return false;
            }
            else
            {
                if (staff.Skills == null) return false;

                bool found = false;
                if (staff.Team != null)
                {
                    //for (int i = 0; i < staff.Team.Club.Directors.Count; ++i)
                    //{
                    //    if (staff.ID.Equals(staff.Team.Club.Directors[i].ID))
                    //    {
                    //        staff_role = "Director";
                    //        found = true;
                    //        break;
                    //    }
                    //}
                }

                if (found)
                {
                    if (return_after_role) return true;

                    if (selectedItem.Equals(staff_role)) return true;
                    return false;
                }

                sbyte comp = (sbyte)(16);
                if (staff.Skills.Manager >= comp)
                    staff_role = staffRoles[ScoutLocalization.R_MANAGER];
                if (staff.Skills.AssistantManager >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += staffRoles[ScoutLocalization.R_ASSISTANTMANAGER];
                }
                if (staff.Skills.Coach >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += staffRoles[ScoutLocalization.R_COACH];
                }
                if (staff.Skills.FitnessCoach >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += staffRoles[ScoutLocalization.R_FITNESSCOACH];
                }
                if (staff.Skills.GoalkeepingCoach >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += staffRoles[ScoutLocalization.R_GOAlKEEPINGCOACH];
                }
                if (staff.Skills.Physio >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += staffRoles[ScoutLocalization.R_PHYSIO];
                }
                if (staff.Skills.Scout >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += staffRoles[ScoutLocalization.R_SCOUT];
                }

                if (return_after_role) return true;
                char[] sep = { ',' };
                string[] roles = staff_role.Split(sep);
                foreach (string role in roles)
                {
                    if (selectedItem.Equals(staffRoles[ScoutLocalization.R_1STYOUTHCOACH]))
                    {
                        if (staff_role.Equals(staffRoles[ScoutLocalization.R_1STTEAMCOACH]) ||
                            staff_role.Equals(staffRoles[ScoutLocalization.R_YOUTHCOACH]) ||
                            staff_role.Equals(staffRoles[ScoutLocalization.R_COACH]))
                            return true;
                    }
                    else if (selectedItem.Equals(role)) return true;
                }
                return false;
            }
        }
		
		
        float GKcoef = 1 / (0.4f + 0.4f + 0.6f - 0.2f + 1.0f + 0.4f + 1.0f + 0.2f + 0.4f + 0.1f + 0.6f + 0.1f + 0.2f + 0.1f
                + 0.4f + 0.1f + 0.8f + 0.2f + 0.2f + 0.8f + 0.2f + -0.1f);
		
		float DCcoef = 1 / (0.1f + 1.0f + 0.8f + 0.2f + 1.0f + 0.2f + 0.2f + 0.4f + 0.6f + 0.2f + 0.4f +  0.2f + 0.8f + 0.4f +
                0.2f + 0.4f + 0.2f + 0.2f + 0.8f + 0.4f + 0.2f + 0.8f - 1.0f + 0.5f + 0.5f);

        float DRcoef = 1 / (0.4f + 0.2f + 0.1f + 0.4f + 0.6f + 0.2f + 1.0f + 0.2f + 0.2f + 0.8f + 0.4f + 0.2f + 0.4f + 0.2f +
                0.4f + 0.1f + 1.0f + 0.2f + 0.2f + 0.6f + 0.2f + 0.2f + 0.6f + 0.6f + 0.6f + 0.6f - 1.0f + 0.2f + 0.8f);

        float DLcoef = 1 / (0.4f + 0.2f + 0.1f + 0.4f + 0.6f + 0.2f + 1.0f + 0.2f + 0.2f + 0.8f + 0.4f + 0.2f + 0.4f + 0.2f +
                0.4f + 0.1f + 1.0f + 0.2f + 0.2f + 0.6f + 0.2f + 0.2f + 0.6f + 0.6f + 0.6f + 0.6f - 1.0f + 0.2f + 0.8f);

        float DMCcoef = 1 / (0.1f + 0.2f + 0.1f + 0.2f + 0.2f + 0.2f + 0.6f + 1.0f + 0.4f + 0.2f + 0.6f + 0.2f + 0.3f + 0.2f +
                0.4f + 0.4f + 0.1f + 0.2f + 0.8f + 0.3f + 0.8f + 0.6f + 0.2f + 0.2f + 0.2f + 0.6f + 0.8f + 0.6f - 1.0f + 0.5f + 0.5f);

       	float DMRcoef = 1 / (0.6f + 0.4f + 0.1f + 0.2f + 0.2f + 0.4f + 0.4f + 0.8f + 0.4f + 0.2f + 0.8f + 0.2f + 0.2f +
                0.2f + 0.2f + 0.4f + 0.1f + 0.2f + 1.0f + 0.2f + 0.3f + 0.8f + 0.2f + 0.2f + 0.2f + 0.8f + 0.8f +
                0.4f - 1.0f + 0.2f + 0.8f);

        float DMLcoef = 1 / (0.6f + 0.4f + 0.1f + 0.2f + 0.2f + 0.4f + 0.4f + 0.8f + 0.4f + 0.2f + 0.8f + 0.2f + 0.2f +
                0.2f + 0.2f + 0.4f + 0.1f + 0.2f + 1.0f + 0.2f + 0.3f + 0.8f + 0.2f + 0.2f + 0.2f + 0.8f + 0.8f +
                0.4f - 1.0f + 0.2f + 0.8f);

        float AMCcoef = 1 / (0.1f + 0.6f + 0.4f + 0.3f + 0.1f + 1.0f + 0.2f + 0.8f + 0.4f + 0.4f + 0.1f + 1.0f + 0.4f + 0.1f +
                0.6f + 0.2f + 0.4f + 0.4f + 0.8f + 0.2f + 0.2f + 0.8f + 0.6f + 0.2f + -1.0f + 0.5f + 0.5f);
        float AMRcoef = 1 / (1.0f + 1.0f + 0.4f + 0.3f + 0.1f + 0.4f + 0.2f + 0.8f + 0.4f + 0.4f + 0.1f + 0.4f + 0.2f + 0.1f +
                0.8f + 0.2f + 0.2f + 0.4f + 1.0f + 0.2f + 0.2f + 1.0f + 0.6f + 0.2f - 1.0f + 0.2f + 0.8f);
        float AMLcoef = 1 / (1.0f + 1.0f + 0.4f + 0.3f + 0.1f + 0.4f + 0.2f + 0.8f + 0.4f + 0.4f + 0.1f + 0.4f + 0.2f + 0.1f +
                0.8f + 0.2f + 0.2f + 0.4f + 1.0f + 0.2f + 0.2f + 1.0f + 0.6f + 0.2f - 1.0f + 0.2f + 0.8f);

        float FCFastcoef = 1 / (0.2f + 0.8f + 1.0f + 0.4f + 0.4f + 0.6f + 0.8f + 0.4f + 0.6f + 0.1f + 0.4f + 0.2f + 0.1f +
                0.8f + 0.1f + 0.2f + 1.0f + 0.4f + 0.2f + 0.2f + 1.0f + 0.4f + 0.2f - 1.0f + 0.5f + 0.5f);
        float FCTargetcoef = 1 / (0.2f + 0.4f + 0.8f + 0.4f + 1.0f + 0.4f + 0.4f + 0.4f + 0.2f + 0.4f + 0.1f + 0.2f + 0.2f +
                0.1f + 1.0f + 0.2f + 0.1f + 0.2f + 0.5f + 0.4f + 0.2f + 1.0f + 0.5f + 0.2f + 1.0f - 1.0f + 0.5f + 0.5f);
		
        internal void calculatePlayerPR(Player player)
        {
            ++playersPRTotal;

            ObservableCollection<String> bestprs = globalFuncs.localization.bestprs;
            ObservableCollection<String> ProfileGenericLabels = globalFuncs.localization.ProfileGenericLabels;

            PositionalRatings pr = new PositionalRatings();

            // GK
            pr.GK += (int)player.Skills.AerialAbility * 0.4f
            + (int)player.Skills.CommandOfArea * 0.4f
            + (int)player.Skills.Communication * 0.6f
            + (int)player.Skills.Eccentricity * -0.2f
            + (int)player.Skills.Handling * 1.0f
            + (int)player.Skills.OneOnOnes * 0.4f
            + (int)player.Skills.Reflexes * 1.0f

            + (int)player.Skills.Acceleration * 0.2f
            + (int)player.Skills.Agility * 0.4f
            + (int)player.Skills.Balance * 0.1f
            + (int)player.Skills.Jumping * 0.6f
            + (int)player.Skills.NaturalFitness * 0.1f
            + (int)player.Skills.Strength * 0.2f

            + (int)player.Skills.Anticipation * 0.1f
            + (int)player.Skills.Bravery * 0.4f
            + (int)player.Skills.Composure * 0.1f
            + (int)player.Skills.Concentration * 0.8f
            + (int)player.Skills.Decisions * 0.2f
            + (int)player.Skills.Influence * 0.2f
            + (int)player.Skills.Positioning * 0.8f
            + (int)player.Skills.Teamwork * 0.2f
            + (int)player.Skills.InjuryProness * -1.0f;

            if (player.Height > 190)
                pr.GK += 100;
            else if (player.Height > 180)
                pr.GK += 50;

            pr.GK *= GKcoef;

            pr.bestPos = bestprs[ScoutLocalization.BP_GK];
            pr.bestPosR = pr.GK;

            // DC
            pr.DC += (int)player.Skills.FirstTouch * 0.1f
            + (int)player.Skills.Heading * 1.0f
            + (int)player.Skills.Marking * 0.8f
            + (int)player.Skills.Passing * 0.2f
            + (int)player.Skills.Tackling * 1.0f
            + (int)player.Skills.Technique * 0.2f

            + (int)player.Skills.Aggression * 0.2f
            + (int)player.Skills.Anticipation * 0.4f
            + (int)player.Skills.Bravery * 0.6f
            + (int)player.Skills.Composure * 0.2f
            + (int)player.Skills.Concentration * 0.4f
            + (int)player.Skills.Influence * 0.2f
            + (int)player.Skills.Positioning * 0.8f
            + (int)player.Skills.Teamwork * 0.4f
            + (int)player.Skills.Workrate * 0.2f

            + (int)player.Skills.Acceleration * 0.4f
            + (int)player.Skills.Agility * 0.2f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Jumping * 0.8f
            + (int)player.Skills.Pace * 0.4f
            + (int)player.Skills.Stamina * 0.2f
            + (int)player.Skills.Strength * 0.8f

            + (int)player.Skills.InjuryProness * -1.0f
			
			+ (int)player.Skills.LeftFoot * 0.5f
            + (int)player.Skills.RightFoot * 0.5f;
			
            if (player.Height > 190)
                pr.DC += 100;
            else if (player.Height > 180)
                pr.DC += 50;
            pr.DC *= DCcoef;

            if (pr.DC > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_DC];
                pr.bestPosR = pr.DC;
            }

            // DRL
            pr.DR += (int)player.Skills.Crossing * 0.4f
            + (int)player.Skills.Dribbling * 0.2f
            + (int)player.Skills.FirstTouch * 0.1f
            + (int)player.Skills.Heading * 0.4f
            + (int)player.Skills.Marking * 0.6f
            + (int)player.Skills.Passing * 0.2f
            + (int)player.Skills.Tackling * 1.0f
            + (int)player.Skills.Technique * 0.2f

            + (int)player.Skills.Aggression * 0.2f
            + (int)player.Skills.Anticipation * 0.8f
            + (int)player.Skills.Bravery * 0.4f
            + (int)player.Skills.Composure * 0.2f
            + (int)player.Skills.Concentration * 0.4f
            + (int)player.Skills.Creativity * 0.2f
            + (int)player.Skills.Decisions * 0.4f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.Positioning * 1.0f
            + (int)player.Skills.Teamwork * 0.2f
            + (int)player.Skills.Workrate * 0.2f

            + (int)player.Skills.Acceleration * 0.6f
            + (int)player.Skills.Agility * 0.2f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Jumping * 0.6f
            + (int)player.Skills.Pace * 0.6f
            + (int)player.Skills.Stamina * 0.6f
            + (int)player.Skills.Strength * 0.6f

            + (int)player.Skills.InjuryProness * -1.0f;

            pr.DL = pr.DR;

            pr.DR += (int)player.Skills.RightFoot * 0.8f;
			pr.DR += (int)player.Skills.LeftFoot * 0.2f; 
            pr.DL += (int)player.Skills.RightFoot * 0.2f;
			pr.DL += (int)player.Skills.LeftFoot * 0.8f;
			
            pr.DR *= DRcoef;
            pr.DL *= DLcoef;

            if (pr.DR > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_DR];
                pr.bestPosR = pr.DR;
                if ((int)player.Skills.Creativity > pr.DR &&
                    (int)player.Skills.Crossing > pr.DR &&
                    (int)player.Skills.Dribbling > pr.DR)
                {
                    pr.bestPos = ProfileGenericLabels[ScoutLocalization.PG_ATT] + " " + bestprs[ScoutLocalization.BP_DR];
                }
            }
            if (pr.DL > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_DL];
                pr.bestPosR = pr.DL;
                if ((int)player.Skills.Creativity > pr.DL &&
                    (int)player.Skills.Crossing > pr.DL &&
                    (int)player.Skills.Dribbling > pr.DL)
                {
                    pr.bestPos = ProfileGenericLabels[ScoutLocalization.PG_ATT] + " " + bestprs[ScoutLocalization.BP_DL];
                }
            }

            // DMC
            pr.DMC += (int)player.Skills.Crossing * 0.1f
            + (int)player.Skills.Dribbling * 0.2f
            + (int)player.Skills.Finishing * 0.1f
            + (int)player.Skills.FirstTouch * 0.2f
            + (int)player.Skills.Heading * 0.2f
            + (int)player.Skills.Marking * 0.2f
            + (int)player.Skills.Passing * 0.6f
            + (int)player.Skills.Tackling * 1.0f
            + (int)player.Skills.Technique * 0.4f

            + (int)player.Skills.Aggression * 0.2f
            + (int)player.Skills.Anticipation * 0.6f
            + (int)player.Skills.Bravery * 0.2f
            + (int)player.Skills.Composure * 0.3f
            + (int)player.Skills.Concentration * 0.2f
            + (int)player.Skills.Creativity * 0.4f
            + (int)player.Skills.Decisions * 0.4f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.OffTheBall * 0.2f
            + (int)player.Skills.Positioning * 0.8f
            + (int)player.Skills.Teamwork * 0.3f
            + (int)player.Skills.Workrate * 0.8f

            + (int)player.Skills.Acceleration * 0.6f
            + (int)player.Skills.Agility * 0.2f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Jumping * 0.2f
            + (int)player.Skills.Pace * 0.6f
            + (int)player.Skills.Stamina * 0.8f
            + (int)player.Skills.Strength * 0.6f

            + (int)player.Skills.InjuryProness * -1.0f

            + (int)player.Skills.LeftFoot * 0.5f
            + (int)player.Skills.RightFoot * 0.5f;
			
            pr.DMC *= DMCcoef;
            if (pr.DMC > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_DMC];
                pr.bestPosR = pr.DMC;
                if ((int)player.Skills.Creativity > pr.DMC &&
                    (int)player.Skills.Flair > pr.DMC &&
                    (int)player.Skills.Technique > pr.DMC)
                {
                    pr.bestPos = bestprs[ScoutLocalization.BP_DMC] + " " + ProfileGenericLabels[ScoutLocalization.PG_PLAYMAKER];
                }
            }

            // DMRL
            pr.DMR += (int)player.Skills.Crossing * 0.6f
            + (int)player.Skills.Dribbling * 0.4f
            + (int)player.Skills.Finishing * 0.1f
            + (int)player.Skills.FirstTouch * 0.2f
            + (int)player.Skills.Heading * 0.2f
            + (int)player.Skills.Marking * 0.4f
            + (int)player.Skills.Passing * 0.4f
            + (int)player.Skills.Tackling * 0.8f
            + (int)player.Skills.Technique * 0.4f

            + (int)player.Skills.Aggression * 0.2f
            + (int)player.Skills.Anticipation * 0.8f
            + (int)player.Skills.Bravery * 0.2f
            + (int)player.Skills.Composure * 0.2f
            + (int)player.Skills.Concentration * 0.2f
            + (int)player.Skills.Creativity * 0.2f
            + (int)player.Skills.Decisions * 0.4f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.OffTheBall * 0.2f
            + (int)player.Skills.Positioning * 1.0f
            + (int)player.Skills.Teamwork * 0.2f
            + (int)player.Skills.Workrate * 0.3f

            + (int)player.Skills.Acceleration * 0.8f
            + (int)player.Skills.Agility * 0.2f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Jumping * 0.2f
            + (int)player.Skills.Pace * 0.8f
            + (int)player.Skills.Stamina * 0.8f
            + (int)player.Skills.Strength * 0.4f

            + (int)player.Skills.InjuryProness * -1.0f;

            pr.DML = pr.DMR;

            pr.DMR += (int)player.Skills.RightFoot * 0.8f;
            pr.DMR += (int)player.Skills.LeftFoot * 0.2f; 
            pr.DML += (int)player.Skills.RightFoot * 0.2f;			
            pr.DML += (int)player.Skills.LeftFoot * 0.8f;
			
            pr.DMR *= DMRcoef;
            pr.DML *= DMLcoef;

            if (pr.DMR > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_DMR];
                pr.bestPosR = pr.DMR;
            }
            if (pr.DML > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_DML];
                pr.bestPosR = pr.DML;
            }

            // AMC
            pr.AMC += (int)player.Skills.Crossing * 0.1f
            + (int)player.Skills.Dribbling * 0.6f
            + (int)player.Skills.Finishing * 0.4f
            + (int)player.Skills.FirstTouch * 0.3f
            + (int)player.Skills.Heading * 0.1f
            + (int)player.Skills.Passing * 1.0f
            + (int)player.Skills.Tackling * 0.2f
            + (int)player.Skills.Technique * 0.8f

            + (int)player.Skills.Anticipation * 0.4f
            + (int)player.Skills.Composure * 0.4f
            + (int)player.Skills.Concentration * 0.1f
            + (int)player.Skills.Creativity * 1.0f
            + (int)player.Skills.Decisions * 0.4f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.OffTheBall * 0.6f
            + (int)player.Skills.Positioning * 0.2f
            + (int)player.Skills.Teamwork * 0.4f
            + (int)player.Skills.Workrate * 0.4f

            + (int)player.Skills.Acceleration * 0.8f
            + (int)player.Skills.Agility * 0.2f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Pace * 0.8f
            + (int)player.Skills.Stamina * 0.6f
            + (int)player.Skills.Strength * 0.2f

            + (int)player.Skills.InjuryProness * -1.0f

            + (int)player.Skills.LeftFoot * 0.5f
            + (int)player.Skills.RightFoot * 0.5f;
			
            pr.AMC *= AMCcoef;
            if (pr.AMC > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_AMC];
                pr.bestPosR = pr.AMC;
            }

            // AMRL
            pr.AMR += (int)player.Skills.Crossing * 1.0f
            + (int)player.Skills.Dribbling * 1.0f
            + (int)player.Skills.Finishing * 0.4f
            + (int)player.Skills.FirstTouch * 0.3f
            + (int)player.Skills.Heading * 0.1f
            + (int)player.Skills.Passing * 0.4f
            + (int)player.Skills.Tackling * 0.2f
            + (int)player.Skills.Technique * 0.8f

            + (int)player.Skills.Anticipation * 0.4f
            + (int)player.Skills.Composure * 0.4f
            + (int)player.Skills.Concentration * 0.1f
            + (int)player.Skills.Creativity * 0.4f
            + (int)player.Skills.Decisions * 0.2f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.OffTheBall * 0.8f
            + (int)player.Skills.Positioning * 0.2f
            + (int)player.Skills.Teamwork * 0.2f
            + (int)player.Skills.Workrate * 0.4f

            + (int)player.Skills.Acceleration * 1.0f
            + (int)player.Skills.Agility * 0.2f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Pace * 1.0f
            + (int)player.Skills.Stamina * 0.6f
            + (int)player.Skills.Strength * 0.2f

            + (int)player.Skills.InjuryProness * -1.0f;
			
            pr.AML = pr.AMR;

            pr.AMR += (int)player.Skills.RightFoot * 0.8f;
			pr.AMR += (int)player.Skills.LeftFoot * 0.2f;
            pr.AML += (int)player.Skills.RightFoot * 0.2f;
            pr.AML += (int)player.Skills.LeftFoot * 0.8f;

            pr.AMR *= AMRcoef;
            pr.AML *= AMLcoef;

            if (pr.AMR > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_AMR];
                pr.bestPosR = pr.AMR;
            }
            if (pr.AML > pr.bestPosR)
            {
                pr.bestPos = bestprs[ScoutLocalization.BP_AML];
                pr.bestPosR = pr.AML;
            }

            // FC-ST
            pr.FCFast += (int)player.Skills.Crossing * 0.2f
            + (int)player.Skills.Dribbling * 0.8f
            + (int)player.Skills.Finishing * 1.0f
            + (int)player.Skills.FirstTouch * 0.4f
            + (int)player.Skills.Heading * 0.4f
            + (int)player.Skills.Passing * 0.6f
            + (int)player.Skills.Technique * 0.8f

            + (int)player.Skills.Anticipation * 0.4f
            + (int)player.Skills.Composure * 0.6f
            + (int)player.Skills.Concentration * 0.1f
            + (int)player.Skills.Creativity * 0.4f
            + (int)player.Skills.Decisions * 0.2f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.OffTheBall * 0.8f
            + (int)player.Skills.Teamwork * 0.1f
            + (int)player.Skills.Workrate * 0.2f

            + (int)player.Skills.Acceleration * 1.0f
            + (int)player.Skills.Agility * 0.4f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Jumping * 0.2f
            + (int)player.Skills.Pace * 1.0f
            + (int)player.Skills.Stamina * 0.4f
            + (int)player.Skills.Strength * 0.2f

            + (int)player.Skills.InjuryProness * -1.0f

            + (int)player.Skills.LeftFoot * 0.5f
            + (int)player.Skills.RightFoot * 0.5f;

            pr.FCFast *= FCFastcoef;

            if (pr.FCFast > pr.bestPosR)
            {
                pr.bestPos = globalFuncs.localization.bestprs[ScoutLocalization.BP_FASTFC];
                pr.bestPosR = pr.FCFast;
            }

            pr.FCTarget += (int)player.Skills.Crossing * 0.2f
            + (int)player.Skills.Dribbling * 0.4f
            + (int)player.Skills.Finishing * 0.8f
            + (int)player.Skills.FirstTouch * 0.4f
            + (int)player.Skills.Heading * 1.0f
            + (int)player.Skills.Passing * 0.4f
            + (int)player.Skills.Technique * 0.4f

            + (int)player.Skills.Anticipation * 0.4f
            + (int)player.Skills.Bravery * 0.2f
            + (int)player.Skills.Composure * 0.4f
            + (int)player.Skills.Concentration * 0.1f
            + (int)player.Skills.Creativity * 0.2f
            + (int)player.Skills.Decisions * 0.2f
            + (int)player.Skills.Influence * 0.1f
            + (int)player.Skills.OffTheBall * 1.0f
            + (int)player.Skills.Positioning * 0.2f
            + (int)player.Skills.Teamwork * 0.1f
            + (int)player.Skills.Workrate * 0.2f

            + (int)player.Skills.Acceleration * 0.5f
            + (int)player.Skills.Agility * 0.4f
            + (int)player.Skills.Balance * 0.2f
            + (int)player.Skills.Jumping * 1.0f
            + (int)player.Skills.Pace * 0.5f
            + (int)player.Skills.Stamina * 0.2f
            + (int)player.Skills.Strength * 1.0f

            + (int)player.Skills.InjuryProness * -1.0f

            + (int)player.Skills.LeftFoot * 0.5f
            + (int)player.Skills.RightFoot * 0.5f;
			
            if (player.Height > 190)
                pr.FCTarget += 100;
            else if (player.Height > 180)
                pr.FCTarget += 50;

            pr.FCTarget *= FCTargetcoef;
            if (pr.FCTarget > pr.bestPosR)
            {
                pr.bestPos = globalFuncs.localization.bestprs[ScoutLocalization.BP_TARGETFC];
                pr.bestPosR = pr.FCTarget;
            }

            playersPRID.Add(player.ID, pr);
        }

        internal void calcCR(ref int stars, float value, int max, int dec)
        {
            stars = 1;
            for (int i = 10; i > 0; --i)
            {
                if (value >= max)
                {
                    stars = i;
                    return;
                }
                max -= dec;
            }
        }

        internal void calculateStaffCR(Staff staff)
        {
            ++staffCRTotal;

            ObservableCollection<String> staffDisplayRatings = globalFuncs.localization.staffDisplayRatings;
            CoachingRatings cr = new CoachingRatings();

            if (!(staff is HumanManager))
            {
                float ddm = (int)(staff.Skills.Determination * 0.2 + 0.5) +
                    staff.Skills.LevelOfDiscipline + (int)(staff.Skills.Motivating * 0.2 + 0.5);

                // fitness
                float value = (int)(staff.Skills.CoachingFitness * 0.2 + 0.5) * 9 + ddm * 2;
                int max = 270;
                int dec = 30;
                calcCR(ref cr.Fitness, value, max, dec);
                cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_FIT];
                cr.BestCRStars = cr.Fitness;

                // goalkeepers
                value = (int)(staff.Skills.CoachingGoalkeepers * 0.2 + 0.5) * 2 + ddm;
                max = 90;
                dec = 10;
                calcCR(ref cr.Goalkeepers, value, max, dec);

                if (cr.Goalkeepers > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.Goalkeepers;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_GK];
                }
                else if (cr.Goalkeepers == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_GK];

                // ball control
                value = (int)(staff.Skills.CoachingTechnical * 0.2 + 0.5) * 6 + (int)(staff.Skills.CoachingMental * 0.2 + 0.5) * 3 + ddm * 2;
                max = 270;
                dec = 30;
                calcCR(ref cr.BallControl, value, max, dec);

                if (cr.BallControl > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.BallControl;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_BC];
                }
                else if (cr.BallControl == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_BC];

                // tactics
                value = (int)(staff.Skills.CoachingTactical * 0.2 + 0.5) * 2 + ddm;
                max = 90;
                dec = 10;
                calcCR(ref cr.Tactics, value, max, dec);

                if (cr.Tactics > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.Tactics;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_TAC];
                }
                else if (cr.Tactics == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_TAC];

                // defending
                value = (int)(staff.Skills.CoachingDefending * 0.2 + 0.5) * 8 + ((int)(staff.Skills.CoachingTactical * 0.2 + 0.5) + ddm) * 3;
                max = 360;
                dec = 40;
                calcCR(ref cr.Defending, value, max, dec);

                if (cr.Defending > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.Defending;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_DEF];
                }
                else if (cr.Defending == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_DEF];

                // attacking
                value = (int)(staff.Skills.CoachingAttacking * 0.2 + 0.5) * 6 + (int)(staff.Skills.CoachingTactical * 0.2 + 0.5) * 3 + ddm * 2;
                max = 270;
                dec = 30;
                calcCR(ref cr.Attacking, value, max, dec);

                if (cr.Attacking > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.Attacking;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_ATT];
                }
                else if (cr.Attacking == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_ATT];

                // shooting
                value = (int)(staff.Skills.CoachingTechnical * 0.2 + 0.5) * 6 + (int)(staff.Skills.CoachingAttacking * 0.2 + 0.5) * 3 + ddm * 2;
                max = 270;
                dec = 30;
                calcCR(ref cr.Shooting, value, max, dec);

                if (cr.Shooting > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.Shooting;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_SHOOT];
                }
                else if (cr.Shooting == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_SHOOT];

                // set pieces
                value = ((int)(staff.Skills.CoachingAttacking * 0.2 + 0.5) + (int)(staff.Skills.CoachingMental * 0.2 + 0.5) +
                         (int)(staff.Skills.CoachingTechnical * 0.2 + 0.5)) * 3 + ddm * 2;
                max = 270;
                dec = 30;
                calcCR(ref cr.SetPieces, value, max, dec);

                if (cr.SetPieces > cr.BestCRStars)
                {
                    cr.BestCRStars = cr.SetPieces;
                    cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_SET];
                }
                else if (cr.SetPieces == cr.BestCRStars) cr.BestCR += "/" + staffDisplayRatings[ScoutLocalization.SD_SET];
            }
            else
            {
                cr.Attacking = 1;
                cr.BallControl = 1;
                cr.Defending = 1;
                cr.Fitness = 1;
                cr.Goalkeepers = 1;
                cr.SetPieces = 1;
                cr.Shooting = 1;
                cr.Tactics = 1;
                cr.BestCR = staffDisplayRatings[ScoutLocalization.SD_ATT];
                cr.BestCRStars = 1;
            }

            staffCRID.Add(staff.ID, cr);
        }
    }
}
