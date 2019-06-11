﻿namespace CalculatorChatBot.Dialogs.Statistics
{
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Serializable]
    public class VarianceDialog : IDialog<object>
    {
        #region Dialog properties
        public string InputString { get; set; }
        public string[] InputStringArray { get; set; }
        public int[] InputInts { get; set; } 
        #endregion

        public VarianceDialog(Activity incomingActivity)
        {
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            if (!string.IsNullOrEmpty(incomingInfo[1]))
            {
                InputString = incomingInfo[1];
                InputStringArray = InputString.Split(',');
                InputInts = Array.ConvertAll(InputStringArray, int.Parse); 
            }
        }

        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var operationType = CalculationTypes.Statistical;
            if (InputInts.Length > 1)
            {
                int sum = InputInts[0];
                foreach (int item in InputInts)
                {
                    sum += item;
                }

                double mean = Convert.ToDouble(sum) / InputInts.Length;
                decimal variance = CalculateVariance(mean, InputInts);
                var successResultType = ResultTypes.Variance;

                #region Building the results 
                var results = new OperationResults()
                {
                    Input = InputString,
                    NumericalResult = decimal.Round(variance, 2).ToString(),
                    OutputMsg = $"Given the list: {InputString}; the variance = {decimal.Round(variance, 2)}",
                    OperationType = operationType.GetDescription(),
                    ResultType = successResultType.GetDescription()
                };

                IMessageActivity resultsReply = context.MakeMessage();
                var resultsAdaptiveCard = OperationResultsAdaptiveCard.GetCard(results);
                resultsReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(resultsAdaptiveCard)
                    }
                };            
                #endregion

                await context.PostAsync(resultsReply);
            }
            else
            {
                var errorResType = ResultTypes.Error;
                var errorResults = new OperationResults()
                {
                    Input = InputString,
                    NumericalResult = "0",
                    OutputMsg = "Your list may be too small to calculate the variance. Please try again later.",
                    OperationType = operationType.GetDescription(),
                    ResultType = errorResType.GetDescription()
                };

                IMessageActivity errorReply = context.MakeMessage();
                var errorReplyAdaptiveCard = OperationErrorAdaptiveCard.GetCard(errorResults);
                errorReply.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(errorReplyAdaptiveCard)
                    }
                };
                await context.PostAsync(errorReply);
            }

            // Popping back to the root dialog
            context.Done<object>(null);
        }

        /// <summary>
        /// This is the method that would calculate the variance among a list
        /// of numbers
        /// </summary>
        /// <param name="mean">The average</param>
        /// <param name="inputInts">The input list of integers</param>
        /// <returns>A decimal value that represents the variance</returns>
        private decimal CalculateVariance(double mean, int[] inputInts)
        {
            double squareDiffs = 0;
            int n = inputInts.Length;

            for (int i = 0; i < inputInts.Length; i++)
            {
                squareDiffs += Math.Pow(Math.Abs(Convert.ToDouble(inputInts[i]) - mean), 2);
            }

            return Convert.ToDecimal(squareDiffs / n);
        }
    }
}