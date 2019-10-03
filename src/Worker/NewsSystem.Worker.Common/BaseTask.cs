﻿namespace NewsSystem.Worker.Common
{
    using System;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using NewsSystem.Data.Models;

    public abstract class BaseTask<TInput, TOutput> : ITask
        where TInput : BaseTaskInput, new()
        where TOutput : BaseTaskOutput, new()
    {
        public async Task<string> DoWork(string parameters)
        {
            var taskParameters = JsonConvert.DeserializeObject<TInput>(parameters) ?? new TInput();
            TOutput taskResult;
            try
            {
                taskResult = await this.DoWork(taskParameters);
            }
            catch (Exception ex)
            {
                taskResult = new TOutput { Ok = false, Error = ex.ToString() };
            }

            var taskResultAsString = JsonConvert.SerializeObject(taskResult);
            return taskResultAsString;
        }

        public WorkerTask Recreate(WorkerTask currentTask)
        {
            var currentParameters = JsonConvert.DeserializeObject<TInput>(currentTask.Parameters ?? string.Empty);
            var currentResult = JsonConvert.DeserializeObject<TOutput>(currentTask.Result ?? string.Empty);
            return currentParameters.Recreate ? this.Recreate(currentTask, currentParameters, currentResult) : null;
        }

        protected virtual WorkerTask Recreate(WorkerTask currentTask, TInput currentParameters, TOutput currentResult) => null; // Returning null means no recreation

        protected abstract Task<TOutput> DoWork(TInput input);

    }
}
