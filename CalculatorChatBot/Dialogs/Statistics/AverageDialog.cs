﻿namespace CalculatorChatBot.Dialogs.Statistics
{
    using CalculatorChatBot.Cards;
    using CalculatorChatBot.Models;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Serializable]
    public class AverageDialog : IDialog<object>
    {
        #region Dialog properties
        public string InputString { get; set; }
        public string[] InputStringArray { get; set; }
        public int[] InputInts { get; set; }
        #endregion

        public AverageDialog(Activity incomingActivity)
        {
            // Extract the incoming text/message
            string[] incomingInfo = incomingActivity.Text.Split(' ');

            // What is the properties to be set for the necessary 
            // operation to be performed
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

            if (InputInts.Length > 1)
            {
                int sum = InputInts[0];
                for (int i = 1; i < InputInts.Length; i++)
                {
                    sum += InputInts[i];
                }

                decimal mean = Convert.ToDecimal(sum) / InputInts.Length;

                #region Building the results object and the card
                var results = new OperationResults()
                {
                    Input = InputString, 
                    Output = decimal.Round(mean, 2).ToString(), 
                    OutputMsg = $"Given the list: {InputString}; the average = {decimal.Round(mean, 2)}",
                    OperationType = CalculationTypes.Statistical.ToString(),
                    ResultType = ResultTypes.Average.ToString()
                };

                IMessageActivity opsReply = context.MakeMessage();
                opsReply.Attachments = new List<Attachment>();

                var opsCard = new OperationResultsCard(results);
                opsReply.Attachments.Add(opsCard.ToAttachment());
                #endregion

                await context.PostAsync(opsReply);
            }
            else
            {
                #region Building the error object
                var errorResults = new OperationResults()
                {
                    Input = InputString,
                    Output = "0",
                    OutputMsg = "Your list may be too small to calculate an average. Please try again later.",
                    OperationType = CalculationTypes.Statistical.ToString(),
                    ResultType = ResultTypes.Error.ToString()
                };

                IMessageActivity errorReply = context.MakeMessage();
                errorReply.Attachments = new List<Attachment>();

                var errorOpsCard = new OperationErrorCard(errorResults);
                errorReply.Attachments.Add(errorOpsCard.ToAttachment());
                #endregion

                await context.PostAsync(errorReply);
            }

            // Return back to the RootDialog
            context.Done<object>(null);
        }
    }
}