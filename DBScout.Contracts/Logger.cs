using System;
using log4net;
using Prism.Logging;

namespace DBScout.Contracts
{
    public class Logger : ILoggerFacade
    {
        private ILog _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger("xyz");
        }

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;
                case Category.Exception:
                    if (priority == Priority.High)
                    {
                        _logger.Fatal(message);
                    }
                    else
                    {
                        _logger.Error(message);
                    }
                    break;
                case Category.Info:
                    _logger.Info(message);
                    break;
                case Category.Warn:
                    _logger.Warn(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }
        }
    }
}