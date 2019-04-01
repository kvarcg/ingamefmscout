using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMScout
{
    public abstract class SearchUI
    {
    }

    public class SearchUIPlayer
    {
        public const int maxAttributes = 134;
        public String fullname;
        public String nation;
        public String club;
        public List<String> player_positions = new List<String>();
        public List<String> player_sides = new List<String>();
        public int regionIndex;
        public String regionItem;
        public int ownershipIndex;
        public int contractStatusIndex;
        public int euIndex;
        public int regenIndex;
        public int prefFootIndex;
        public int bestprIndex;
        public String bestprItem;
        public int wageMin;
        public int wageMax;
        public int prMin;
        public int prMax;
        public int prMaximum;
        public long gameDate;
        public int[] numericUpDownArray = new int[maxAttributes];
    }

    public class SearchUIStaff
    {
        public const int maxAttributes = 118;
        public String fullname;
        public String nation;
        public String club;
        public int roleIndex;
        public String role;
        public int regionIndex;
        public String regionItem;
        public int contractStatusIndex;
        public int regenIndex;
        public int bestcrIndex;
        public String bestcrItem;
        public int fitnessMin;
        public int fitnessMax;
        public int goalkeepersMin;
        public int goalkeepersMax;
        public int ballControlMin;
        public int ballControlMax;
        public int tacticsMin;
        public int tacticsMax;
        public int defendingMin;
        public int defendingMax;
        public int attackingMin;
        public int attackingMax;
        public int shootingMin;
        public int shootingMax;
        public int setPiecesMin;
        public int setPiecesMax;
        public long gameDate;
        public int[] numericUpDownArray = new int[maxAttributes];
    }

    public class SearchUITeam
    {
        public String name;
        public String nation;
        public String stadium;
        public int teamtypeIndex;
        public String teamtypeItem;
        public int regionIndex;
        public String regionItem;
        public int reputationIndex;
        public String reputationItem;
        public int transferBudgetMin;
        public int transferBudgetMax;
        public int wageBudgetMin;
        public int wageBudgetMax;
    }
}
