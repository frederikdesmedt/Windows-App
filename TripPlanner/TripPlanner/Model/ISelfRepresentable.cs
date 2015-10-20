using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace TripPlanner.Model
{
    public interface ISelfRepresentable<T>
    {
        T Self { get; }
    }
}
