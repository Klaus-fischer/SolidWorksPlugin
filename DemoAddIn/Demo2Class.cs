// <copyright file="Demo2Class.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.DemoAddin
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections;
    using System.Reflection;

    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swpublished;
    using SolidWorks.Interop.swconst;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ComponentModel;
    using SIM.SolidWorksPlugin;

    [Guid("217A321E-E54D-4658-ABEB-EDE9E15A86CF")]
    [ComVisible(true)]
    [Description("Demo 2 description")]
    [DisplayName("Demo 2")]
    public class Demo2Class : ISwAddin
    {
        ISldWorks iSwApp = null;
        ICommandManager iCmdMgr = null;
        ICommandGroup cmdGroup;
        IFlyoutGroup flyoutGroup;
        int addinID = 0;

        public const int mainCmdGroupID = 5;
        public const int mainItemID1 = 0;
        public const int mainItemID2 = 1;
        public const int mainItemID3 = 2;
        public const int flyoutGroupID = 91;

        string[] mainIcons = new string[3];
        string[] icons = new string[3];


        // Public Properties
        public ISldWorks SwApp
        {
            get { return iSwApp; }
        }

        public ICommandManager CmdMgr
        {
            get { return iCmdMgr; }
        }

        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type t) => SwComInterop.RegisterFunction(t);

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type t) => SwComInterop.UnregisterFunction(t);

        public Demo2Class()
        {
        }

        public bool ConnectToSW(object ThisSW, int cookie)
        {
            iSwApp = (ISldWorks)ThisSW;
            addinID = cookie;

            //Set up callbacks
            iSwApp.SetAddinCallbackInfo2(0, this, addinID);

            iCmdMgr = iSwApp.GetCommandManager(cookie);
            AddCommandMgr();

            return true;
        }

        public bool DisconnectFromSW()
        {
            RemoveCommandMgr();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(iCmdMgr);
            iCmdMgr = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(iSwApp);
            iSwApp = null;
            // The add-in must call GC.Collect() here in order to retrieve all managed code pointers 
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        #region UI Methods
        public void updateBtns()
        {

            flyoutGroup = iCmdMgr.GetFlyoutGroup(91);

            flyoutGroup.RemoveAllCommandItems();

            flyoutGroup.AddCommandItem("FlyoutCommand 1", "FlyoutCommand 1", 0, "FlyoutCommandItem1", "FlyoutEnableCommandItem1");
            flyoutGroup.AddCommandItem("FlyoutCommand 2", "FlyoutCommand 2", 0, "FlyoutCommandItem2", "FlyoutEnableCommandItem2");

            flyoutGroup.FlyoutType = (int)swCommandFlyoutStyle_e.swCommandFlyoutStyle_Simple;

            IFlyoutGroup fogrp;
            fogrp = iCmdMgr.GetFlyoutGroup(91);
            Debug.Print("  CmdID: " + fogrp.CmdID);
            Debug.Print("  Button count: " + fogrp.ButtonCount);
            Debug.Print("  Flyout type: " + fogrp.FlyoutType);
            mainIcons = (string[])fogrp.MainIconList;
            Debug.Print("  Small button: " + mainIcons[0]);
            Debug.Print("  Medium button: " + mainIcons[1]);
            Debug.Print("  Large button: " + mainIcons[1]);
            icons = (string[])fogrp.IconList;
            Debug.Print("  Small toolbar button: " + icons[0]);
            Debug.Print("  Medium toolbar button: " + icons[2]);
            Debug.Print("  Large toolbar button: " + icons[2]);

        }
        public void AddCommandMgr()
        {

            Assembly thisAssembly;
            int cmdIndex0, cmdIndex1;
            string Title = "C# Add-in", ToolTip = "Flyout demo";


            int[] docTypes = new int[]{(int)swDocumentTypes_e.swDocASSEMBLY,
                                       (int)swDocumentTypes_e.swDocDRAWING,
                                       (int)swDocumentTypes_e.swDocPART};

            thisAssembly = System.Reflection.Assembly.GetAssembly(this.GetType());


            int cmdGroupErr = 0;
            bool ignorePrevious = false;

            object registryIDs;
            // Get the ID information stored in the registry
            bool getDataResult = iCmdMgr.GetGroupDataFromRegistry(mainCmdGroupID, out registryIDs);

            int[] knownIDs = new int[2] { mainItemID1, mainItemID2 };

            if (getDataResult)
            {
                if (!CompareIDs((int[])registryIDs, knownIDs)) // If the IDs don't match, reset the CommandGroup
                {
                    ignorePrevious = true;
                }
            }

            cmdGroup = iCmdMgr.CreateCommandGroup2(mainCmdGroupID, Title, ToolTip, "", -1, ignorePrevious, ref cmdGroupErr);

            icons[0] = "Pathname_to_toolbar_nxn_image";
            icons[1] = "Pathname_to_toolbar_nnxnn_image";
            icons[2] = "Pathname_to_toolbar_nnnxnnn_image";
            mainIcons[0] = "Pathname_to_nxn_image";
            mainIcons[1] = "Pathname_to_nnxnn_image";
            mainIcons[2] = "Pathname_to_nnnxnnn_image";

            cmdGroup.IconList = icons;
            cmdGroup.MainIconList = mainIcons;

            int menuToolbarOption = (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem);
            cmdIndex0 = cmdGroup.AddCommandItem2("CreateCube", -1, "Create a cube", "Create cube", 0, "CreateCube", "", mainItemID1, menuToolbarOption);
            cmdIndex1 = cmdGroup.AddCommandItem2("Show PMP", -1, "Display sample property manager", "Show PMP", 2, "ShowPMP", "EnablePMP", mainItemID2, menuToolbarOption);

            cmdGroup.HasToolbar = true;
            cmdGroup.HasMenu = true;
            cmdGroup.Activate();

            bool bResult;

            flyoutGroup = iCmdMgr.CreateFlyoutGroup2(
                flyoutGroupID,
                "Dynamic Flyout", "Dynamic Flyout", "Flyout Hint",
                mainIcons, icons, "FlyoutCallback", "FlyoutEnable");

            // Add the FlyoutGroup to the context-sensitive menus of faces in parts
            bResult = flyoutGroup.AddContextMenuFlyout((int)swDocumentTypes_e.swDocPART, (int)swSelectType_e.swSelFACES);
            Debug.Print("Context-sensitive menu flyout created for faces in parts: " + bResult.ToString());

            // Get the total number of FlyoutGroups in CommandManager
            Debug.Print("Number of FlyoutGroups is " + iCmdMgr.NumberOfFlyoutGroups);

            // Get the FlyoutGroups
            object[] objGroups;
            objGroups = (object[])iCmdMgr.GetFlyoutGroups();
            Debug.Print("Find all FlyoutGroups in CommandManager:");
            int i;
            for (i = 0; i <= objGroups.GetUpperBound(0); i++)
            {
                Debug.Print("FlyoutGroup found");

            }

            // Get a FlyoutGroup by its user-defined ID
            IFlyoutGroup fogrp;
            fogrp = iCmdMgr.GetFlyoutGroup(91);
            Debug.Print("  CmdID: " + fogrp.CmdID);
            Debug.Print("  Button count: " + fogrp.ButtonCount);
            Debug.Print("  Flyout Type: " + fogrp.FlyoutType);
            mainIcons = (string[])fogrp.MainIconList;
            Debug.Print("  Small button: " + mainIcons[0]);
            Debug.Print("  Medium button: " + mainIcons[1]);
            Debug.Print("  Large button: " + mainIcons[2]);
            icons = (string[])fogrp.IconList;
            Debug.Print("  Small toolbar button: " + icons[0]);
            Debug.Print("  Medium toolbar button: " + icons[1]);
            Debug.Print("  Large toolbar button: " + icons[2]);


            foreach (int type in docTypes)
            {
                CommandTab cmdTab;

                cmdTab = iCmdMgr.GetCommandTab(type, Title);

                if (cmdTab != null & !getDataResult | ignorePrevious)// If tab exists, but we have ignored the registry info (or changed CommandGroup ID), re-create the tab; otherwise the ids won't match and the tab will be blank
                {
                    bool res = iCmdMgr.RemoveCommandTab(cmdTab);
                    cmdTab = null;
                }

                // If cmdTab is null, must be first load (possibly after reset), add the commands to the tabs
                if (cmdTab == null)
                {
                    cmdTab = iCmdMgr.AddCommandTab(type, Title);

                    CommandTabBox cmdBox = cmdTab.AddCommandTabBox();

                    int[] cmdIDs = new int[3];
                    int[] TextType = new int[3];

                    cmdIDs[0] = cmdGroup.get_CommandID(cmdIndex0);

                    TextType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                    cmdIDs[1] = cmdGroup.get_CommandID(cmdIndex1);

                    TextType[1] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                    cmdIDs[2] = cmdGroup.ToolbarId;

                    TextType[2] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal | (int)swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout;

                    bResult = cmdBox.AddCommands(cmdIDs, TextType);


                    CommandTabBox cmdBox1 = cmdTab.AddCommandTabBox();
                    cmdIDs = new int[1];
                    TextType = new int[1];

                    cmdIDs[0] = flyoutGroup.CmdID;
                    TextType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow | (int)swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout;

                    bResult = cmdBox1.AddCommands(cmdIDs, TextType);

                    cmdTab.AddSeparator(cmdBox1, cmdIDs[0]);

                }

            }
            thisAssembly = null;

        }

        public void RemoveCommandMgr()
        {
            iCmdMgr.RemoveCommandGroup(mainCmdGroupID);
            iCmdMgr.RemoveFlyoutGroup(flyoutGroupID);
        }

        public bool CompareIDs(int[] storedIDs, int[] addinIDs)
        {
            List<int> storedList = new List<int>(storedIDs);
            List<int> addinList = new List<int>(addinIDs);

            addinList.Sort();
            storedList.Sort();

            if (addinList.Count != storedList.Count)
            {
                return false;
            }
            else
            {

                for (int i = 0; i < addinList.Count; i++)
                {
                    if (addinList[i] != storedList[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void CreateCube()
        {
            // Make sure a part open is open
            string partTemplate = iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                IModelDoc2 modDoc = (IModelDoc2)iSwApp.NewDocument(partTemplate, (int)swDwgPaperSizes_e.swDwgPaperA2size, 0.0, 0.0);
                ISketchManager sketchMgr = modDoc.SketchManager;
                sketchMgr.InsertSketch(true);
                sketchMgr.CreateCornerRectangle(0, 0, 0, .1, .1, .1);
                // Extrude the sketch
                IFeatureManager featMan = modDoc.FeatureManager;
                featMan.FeatureExtrusion3(true,
                    false, false,
                    (int)swEndConditions_e.swEndCondBlind, (int)swEndConditions_e.swEndCondBlind,
                    0.1, 0.0,
                    false, false,
                    false, false,
                    0.0, 0.0,
                    false, false,
                    false, false,
                    false,
                    false, true, (int)swStartConditions_e.swStartSketchPlane, 0.0, false);
            }
        }


        public void ShowPMP()
        {

        }

        public int EnablePMP()
        {
            if (iSwApp.ActiveDoc != null)
                return 1;
            else
                return 0;
        }

        public void FlyoutCallback()
        {
            updateBtns();
        }

        public int FlyoutEnable()
        {
            // Enable the flyout only if CommandGroup buttons are enabled
            if (cmdGroup.HasEnabledButton)
                return 1;
            else
                return 0;
        }

        public void FlyoutCommandItem1() { Debug.Print("Flyout command 1 called"); }

        public int FlyoutEnableCommandItem1() { return 1; }

        public void FlyoutCommandItem2() { Debug.Print("Flyout command 2 called"); }

        public int FlyoutEnableCommandItem2() { return 1; }

        #endregion
    }
}
