﻿// <copyright file="DistanceDialog.cs" company="XYZ Software LLC">
// Copyright (c) XYZ Software LLC. All rights reserved.
// </copyright>

namespace CalculatorChatBot.Dialogs.Geometry
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;

    [Serializable]
    public class DistanceDialog : IDialog<object>
    {
        public DistanceDialog(Activity incomingActivity)
        {
            // Parsing through the incoming information
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // Setting all of the properties
            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                this.InputString = incomingInfo[1];
                this.InputStringArray = this.InputString.Split(',');
                this.InputInts = Array.ConvertAll(this.InputStringArray, int.Parse);
            }
        }

        public string[] InputStringArray { get; set; }

        public string InputString { get; set; }

        public int[] InputInts { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentException(nameof(context));
            }

            var operationType = CalculationTypes.Geometric;
            if (this.InputInts.Length > 1 && this.InputInts.Length == 4)
            {
                int x1 = this.InputInts[0];
                int y1 = this.InputInts[1];

                var point1 = $"({x1}, {y1})";

                int x2 = this.InputInts[2];
                int y2 = this.InputInts[3];

                var point2 = $"({x2},{y2})";

                var deltaX = x2 - x1;
                var deltaY = y2 - y1;

                var calculation = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
                var distanceFormula = Convert.ToDecimal(calculation);
                var resultsType = ResultTypes.Distance;
                var successResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = decimal.Round(distanceFormula, 2).ToString(),
                    OutputMsg = $"Given the points: {point1} and {point2}, the distance = {distanceFormula}",
                    OperationType = operationType.GetDescription(),
                    ResultType = resultsType.GetDescription()
                };

                IMessageActivity successReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(successResults);
                successReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };

                await context.PostAsync(successReply);
            }
            else
            {
                var errorResultType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = this.InputString,
                    NumericalResult = "0",
                    OutputMsg = "There needs to be exactly 4 elememts to calculate the midpoint. Please try again later",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResultType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorResultsAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorResultsAdaptiveCard)
                    }
                };
                await context.PostAsync(errorReply);
            }

            // Returning back to the main root dialog stack
            context.Done<object>(null);
        }
    }
}