using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace FMScout
{
    public class Multiplier
    {
        public float multiplier;
        public IFormatProvider format;
        public Multiplier(float multiplier, IFormatProvider format)
        {
            this.multiplier = multiplier;
            this.format = format;
        }
    }

    public class MultiplierExtended : Multiplier
    {
        public string extended;

        public MultiplierExtended(float multiplier, string extended, IFormatProvider format) :
            base(multiplier, format)
        {
            this.extended = extended;
        }
    }

    public class PreferencesSettings
    {
        public bool isDefault = false;
        public bool isCurrent = false;
        public string name;
        public List<int> playerColumns;
        public List<int> staffColumns;
        public List<int> teamColumns;
        public List<int> shortlistColumns;
        public String currency = String.Empty;
        public Multiplier currencyMultiplier = new Multiplier(0.0f, null);
        public String wage = String.Empty;
        public MultiplierExtended wageMultiplier = new MultiplierExtended(0.0f, "", null);
        public String height = String.Empty;
        public MultiplierExtended heightMultiplier = new MultiplierExtended(0.0f, "", null);
        public String weight = String.Empty;
        public MultiplierExtended weightMultiplier = new MultiplierExtended(0.0f, "", null);
        public String editing = String.Empty;
        public String language = String.Empty;
        public String theme = String.Empty;
        public int wonderkidsMaxAge = 0;
        public int wonderkidsMinPA = 0;
        public int wonderstaffMinPA = 0;
        public int wonderteamsMinRep = 0;

        public PreferencesSettings(string name)
        {
            if (!name.Equals("Custom"))
                GlobalSettings.getSettings().settingNames.Add(name);
            this.name = name;
            playerColumns = new List<int>();
            staffColumns = new List<int>();
            teamColumns = new List<int>();
            shortlistColumns = new List<int>();
        }

        public bool Equals(PreferencesSettings other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            if (Object.ReferenceEquals(other, this))
                return true;
            bool element = this.currency == other.currency &&
                           this.wage == other.wage &&
                           this.height == other.height &&
                           this.weight == other.weight &&
                           this.editing == other.editing &&
                           this.language == other.language &&
                           this.theme == other.theme &&
                           this.wonderkidsMaxAge == other.wonderkidsMaxAge &&
                           this.wonderkidsMinPA == other.wonderkidsMinPA &&
                           this.wonderstaffMinPA == other.wonderstaffMinPA &&
                           this.wonderteamsMinRep == other.wonderteamsMinRep;

            bool list = true;
            for (int i = 0; i < this.playerColumns.Count; ++i)
            {
                if (this.playerColumns[i] != other.playerColumns[i])
                {
                    list = false;
                    break;
                }
            }

            for (int i = 0; i < this.staffColumns.Count; ++i)
            {
                if (this.staffColumns[i] != other.staffColumns[i])
                {
                    list = false;
                    break;
                }
            }

            for (int i = 0; i < this.teamColumns.Count; ++i)
            {
                if (this.teamColumns[i] != other.teamColumns[i])
                {
                    list = false;
                    break;
                }
            }

            for (int i = 0; i < this.shortlistColumns.Count; ++i)
            {
                if (this.shortlistColumns[i] != other.shortlistColumns[i])
                {
                    list = false;
                    break;
                }
            }

            return element && list;
        }

        public override bool Equals(object obj)
        {
            if (obj is PreferencesSettings)
            {
                return this.Equals((PreferencesSettings)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() ^ currency.GetHashCode() ^ wage.GetHashCode() ^
                   height.GetHashCode() ^ weight.GetHashCode() ^ language.GetHashCode() ^ theme.GetHashCode() ^
                   playerColumns.GetHashCode() ^ staffColumns.GetHashCode() ^ teamColumns.GetHashCode() ^ shortlistColumns.GetHashCode();
        }

        public static bool operator ==(PreferencesSettings a, PreferencesSettings b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PreferencesSettings a, PreferencesSettings b)
        {
            return !(a == b);
        }

        public void set(PreferencesSettings other)
        {
            this.currency = other.currency;
            this.currencyMultiplier.multiplier = other.currencyMultiplier.multiplier;
            this.currencyMultiplier.format = other.currencyMultiplier.format;
            this.wage = other.wage;
            this.wageMultiplier.multiplier = other.wageMultiplier.multiplier;
            this.wageMultiplier.extended = other.wageMultiplier.extended;
            this.height = other.height;
            this.heightMultiplier.multiplier = other.heightMultiplier.multiplier;
            this.heightMultiplier.extended = other.heightMultiplier.extended;
            this.weight = other.weight;
            this.weightMultiplier.multiplier = other.weightMultiplier.multiplier;
            this.weightMultiplier.extended = other.weightMultiplier.extended;
            this.editing = other.editing;
            this.language = other.language;
            this.theme = other.theme;
            this.wonderkidsMaxAge = other.wonderkidsMaxAge;
            this.wonderkidsMinPA = other.wonderkidsMinPA;
            this.wonderstaffMinPA = other.wonderstaffMinPA;
            this.wonderteamsMinRep = other.wonderteamsMinRep;

            this.playerColumns.Clear();
            this.playerColumns.AddRange(other.playerColumns);
            this.staffColumns.Clear(); 
            this.staffColumns.AddRange(other.staffColumns);
            this.teamColumns.Clear(); 
            this.teamColumns.AddRange(other.teamColumns);
            this.shortlistColumns.Clear(); 
            this.shortlistColumns.AddRange(other.shortlistColumns);
        }
    }
}