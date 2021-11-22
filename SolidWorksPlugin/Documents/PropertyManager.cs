﻿// <copyright file="PropertyManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

#pragma warning disable SA1202 // Elements should be ordered by access
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Linq;
    using SIM.SolidWorksPlugin.Extensions;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The property manager.
    /// </summary>
    internal class PropertyManager : IPropertyManager
    {
        private IModelDoc2 activeModel;
        private string activeConfiguration = string.Empty;

        private CustomPropertyManager SwPropertyManager
            => this.activeModel.Extension.get_CustomPropertyManager(this.activeConfiguration);

        /// <summary>
        /// Gets or sets the active model.
        /// </summary>
        public IModelDoc2 ActiveModel
        {
            get => this.activeModel;
            set
            {
                this.activeModel = value;
                this.activeConfiguration = string.Empty;
            }
        }

        /// <inheritdoc/>
        public string CustomPropertyConfiguration
        {
            get => this.activeConfiguration;
            set => this.SetActiveConfiguration(value);
        }

        /// <inheritdoc/>
        public string Title
        {
            get => this[swSummInfoField_e.swSumInfoTitle];
            set => this[swSummInfoField_e.swSumInfoTitle] = value;
        }

        /// <inheritdoc/>
        public string Subject
        {
            get => this[swSummInfoField_e.swSumInfoSubject];
            set => this[swSummInfoField_e.swSumInfoSubject] = value;
        }

        /// <inheritdoc/>
        public string Author
        {
            get => this[swSummInfoField_e.swSumInfoAuthor];
            set => this[swSummInfoField_e.swSumInfoAuthor] = value;
        }

        /// <inheritdoc/>
        public string Keywords
        {
            get => this[swSummInfoField_e.swSumInfoKeywords];
            set => this[swSummInfoField_e.swSumInfoKeywords] = value;
        }

        /// <inheritdoc/>
        public string Comments
        {
            get => this[swSummInfoField_e.swSumInfoComment];
            set => this[swSummInfoField_e.swSumInfoComment] = value;
        }

        /// <inheritdoc/>
        public string? this[string propertyName]
        {
            get => this.GetStringProperty(propertyName);
            set => this.SetStringProperty(propertyName, value);
        }

        private string this[swSummInfoField_e key]
        {
            get => this.activeModel.SummaryInfo[(int)key];
            set => this.activeModel.SummaryInfo[(int)key] = value;
        }

        /// <inheritdoc/>
        public void SetStringProperty(string propertyName, string? value)
        {
            if (value is null)
            {
                this.SwPropertyManager.Delete2(propertyName);
            }
            else
            {
                this.SwPropertyManager.Add3(
                    FieldName: propertyName,
                    FieldType: (int)swCustomInfoType_e.swCustomInfoText,
                    FieldValue: value,
                    OverwriteExisting: (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
            }

            this.ActiveModel.SetSaveFlag();
        }

        /// <inheritdoc/>
        public string? GetStringProperty(string propertyName)
        {
            swCustomInfoGetResult_e result =
                (swCustomInfoGetResult_e)this.SwPropertyManager.Get5(
                    FieldName: propertyName,
                    UseCached: false,
                    ValOut: out string value,
                    ResolvedValOut: out string resolvedValOut,
                    WasResolved: out _);

            if (result == swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent)
            {
                return null;
            }

            return result == swCustomInfoGetResult_e.swCustomInfoGetResult_ResolvedValue
                ? resolvedValOut
                : value;
        }

        /// <inheritdoc/>
        public void DeleteProperty(string propertyName)
        {
            this.SwPropertyManager.Delete2(propertyName);
            this.ActiveModel.SetSaveFlag();
        }

        /// <inheritdoc/>
        public void SetDateProperty(string propertyName, DateTime? value)
        {
            if (value == null)
            {
                this.SwPropertyManager.Delete2(propertyName);
            }
            else
            {
                this.SwPropertyManager.Add3(
                    FieldName: propertyName,
                    FieldType: (int)swCustomInfoType_e.swCustomInfoDate,
                    FieldValue: value.Value.ToString("dd.MM.yyyy"),
                    OverwriteExisting: (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
            }

            this.ActiveModel.SetSaveFlag();
        }

        /// <inheritdoc/>
        public DateTime? GetDateProperty(string propertyName)
        {
            if (DateTime.TryParse(this.GetStringProperty(propertyName), out DateTime time))
            {
                return time;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public void SetWeight(double weight)
        {
            if (this.ActiveModel is DrawingDoc)
            {
                return;
            }

            MassProperty mass = this.ActiveModel.Extension.CreateMassProperty();

            if (mass.OverrideMass || Math.Abs(mass.Mass - weight) / mass.Mass > 0.05)
            {
                mass.SetOverrideMassValue(
                    Value: weight,
                    Config_option: (int)swInConfigurationOpts_e.swAllConfiguration,
                    Config_names: string.Empty);

                this.ActiveModel.SetSaveFlag();
            }
        }

        /// <inheritdoc/>
        public double GetWeight()
        {
            if (this.ActiveModel is DrawingDoc)
            {
                return -1;
            }

            MassProperty mass = this.ActiveModel.Extension.CreateMassProperty();
            return mass.Mass;
        }

        private void SetActiveConfiguration(string value)
        {
            if (this.ActiveModel is null)
            {
                return;
            }

            if (this.activeModel.GetConfigurationNameStrings().Contains(value))
            {
                this.activeConfiguration = value;
            }
        }
    }
}
