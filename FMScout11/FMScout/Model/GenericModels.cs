using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace FMScout.Model
{
    public static class GenericModels
    {
        public static ObservableCollection<TextModel> getYesNoTextModelList()
        {
            return new ObservableCollection<TextModel>
            {
                new TextModel { TextContent = "Yes" },
                new TextModel { TextContent = "No" }
            };
        }
    }
}
