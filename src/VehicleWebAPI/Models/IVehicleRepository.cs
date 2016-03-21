using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleWebAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Read and Delete can emit data about their success/failure through their return value.
    /// While Create and Update should be as simple as possible given that we're at the model level,
    /// I chose to have them return data about their success/failure as well for consistency. Maybe
    /// someday someone will want that data.
    /// </remarks>
    public interface IVehicleRepository
    {
        
        int Key { get; }

        // Method names a bit on-the-nose

        /// <summary>
        /// Adds a Vehicle to the in-memory storage for Vehicles. Returns 'true' if successful;
        /// otherwise 'false'.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        bool Create(Vehicle v);

        /// <summary>
        /// Returns a Vehicle from the in-memory storage for Vehicles with Id of 'key'.
        /// Returns found Vehicle if successful; otherwise 'null'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Vehicle Read(int key);

        /// <summary>
        /// Updates a Vehicle from the in-memory storage with an Id that matches the argument's Id.
        /// Returns 'true' if Id exists and updates in-memory Vehicle; otherwise returns 'false' and
        /// does not change in-memory state.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <remarks>
        /// Though there are ambiguous definitions about the behavior of PUT, I'll match the test
        /// client's behavior where PUT of a now-deleted resource fails (i.e. only updates;
        /// does not create).
        /// </remarks>
        bool Update(Vehicle v);

        /// <summary>
        /// Deletes a Vehicle from the in-memory storage with Id of 'key'.
        /// Returns deleted Vehicle if successful; otherwise 'null'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Vehicle Delete(int key);

        /// <summary>
        /// Return all Vehicles. Typically for use with GET /{controller}/
        /// </summary>
        /// <returns></returns>
        IEnumerable<Vehicle> GetAllVehicles();
    }
}
