using System.Collections;
using System.Collections.Generic;

namespace Oneeronaut
{
    public interface IAppModel
    {
        IPositioned User { get; set; }

        //List<IPositioned> placedObjects;
        IPositioned PlacedObject { get; set; }
    }
    
    /**
     * The AppModel class servers as a container for the application data.
     * It implements the IAppModel interface.
     */
    public class AppModel : IAppModel
    {
        public IPositioned User { get; set; }
        public IPositioned PlacedObject { get; set; }
    }
    
}