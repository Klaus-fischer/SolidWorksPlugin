// <copyright file="ICommandTabBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;
    using SolidWorks.Interop.sldworks;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICommandTabBuilder
    {
        void AddCommandToTab(ICommandInfo commandInfom, swCommandTabButtonTextDisplay_e textDisplay);

        void AddSpacerToTab();
    }

    public interface ICommandTabManager
    {
        void BuildCommandTab(Action<ICommandTabBuilder> factoryMethod, params swDocumentTypes_e[] documentTypes);
    }

    public class TabCommandManager : ICommandTabManager
    {
        public void BuildCommandTab(Action<ICommandTabBuilder> factoryMethod, params swDocumentTypes_e[] documentTypes)
        {
            throw new NotImplementedException();
        }
    }

    public class CommandTabBuilder : ICommandTabBuilder
    {
        ICommandTabBox swCommandTabBox;

        List<(int CommandID, int DisplayStyle)?> CommandStyles = new();

        public void AddCommandToTab(ICommandInfo commandInfo, swCommandTabButtonTextDisplay_e textDisplayStyle)
        {
            this.CommandStyles.Add((commandInfo.Id, (int)textDisplayStyle));
        }

        public void AddSpacerToTab()
        {
            this.CommandStyles.Add(null);
        }

        public IEnumerable<(int[] CommandIds, int[] DisplayStyles)> GetCommands()
        {
            List<int> commands = new();
            List<int> styles = new();

            for (int i = 0; i < this.CommandStyles.Count; i++)
            {
                var commandStyle = this.CommandStyles[i];

                if (commandStyle.HasValue)
                {
                    commands.Add(commandStyle.Value.CommandID);
                    styles.Add(commandStyle.Value.DisplayStyle);
                }
                else
                {
                    yield return (commands.ToArray(), styles.ToArray());
                    commands.Clear();
                    styles.Clear();
                }
            }
        }
    }
}
