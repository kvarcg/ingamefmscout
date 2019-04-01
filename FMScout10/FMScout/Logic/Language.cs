using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMScout
{
    public class Language
    {
        public String name = "";

        public List<String> currencies = new List<String>();
        public List<String> wages = new List<String>();
        public List<String> heights = new List<String>();
        public List<String> weights = new List<String>();
        public List<String> yesno = new List<String>();
        public List<String> contractstatuses = new List<String>();
        public List<String> regions = new List<String>();
        public List<String> WindowGeneralLabels = new List<String>();
        public List<String> WindowPreferencesLabels = new List<String>();
        public List<String> WindowAboutLabels = new List<String>();
        public List<String> WindowLoadingLabels = new List<String>();
        public List<String> WindowDonateLabels = new List<String>();
        public List<String> WindowInfoLabels = new List<String>();
        public List<String> WindowCustomizeColumnsLabels = new List<String>();
        public List<String> MenuLabels = new List<String>();
        public List<String> GeneralSearchLabels = new List<String>();
        public List<String> WindowMainLabels = new List<String>();
        public List<String> PlayerSearchLabels = new List<String>();
        public List<String> StaffSearchLabels = new List<String>();
        public List<String> TeamSearchLabels = new List<String>();
        public List<String> PlayerProfileLabels = new List<String>();
        public List<String> StaffProfileLabels = new List<String>(); 
        public List<String> SearchingResults = new List<String>();
        public List<String> ProfileGenericLabels = new List<String>();
        public List<String> TeamProfileLabels = new List<String>();
        public List<String> WindowProfileLabels = new List<String>();

        #region players

        public List<String> playerSearchAttributesGroupBoxes = new List<String>();
        public List<String> prefFoots = new List<String>();
        public List<String> bestprs = new List<String>();
        public List<String> ownerShips = new List<String>();

        #endregion

        #region staff

        public List<String> staffSearchAttributesGroupBoxes = new List<String>();
        public List<String> staffDisplayRatings = new List<String>();
        public List<String> staffRoles = new List<String>();
        public List<String> bestcrs = new List<String>();

        #endregion

        #region teams

        public List<String> teamGroupBoxes = new List<String>();
        public List<String> teamtypes = new List<String>();
        public List<String> reputations = new List<String>();

        #endregion

        public List<String> playerColumns = new List<String>();
        public List<String> staffColumns = new List<String>();
        public List<String> teamColumns = new List<String>();
        public List<String> shortlistColumns = new List<String>();
    }
}
