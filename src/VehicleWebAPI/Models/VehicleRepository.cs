using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleWebAPI.Models
{
    public class VehicleRepository : IVehicleRepository
    {
        public static ConcurrentDictionary<int, Vehicle> Vehicles = 
            new ConcurrentDictionary<int, Vehicle>();

        public int Key
        {
            get; private set;
        }
        
        public VehicleRepository()
        {
            Key = 1;
        }

        public bool Create(Vehicle v)
        {
            v.Id = Key;
            var wasSuccess = Vehicles.TryAdd(v.Id, v);
            if (wasSuccess)
            {
                Key++;
            }
            return wasSuccess;
        }

        public Vehicle Read(int key)
        {
            Vehicle v;
            Vehicles.TryGetValue(key, out v);
            return v;
        }

        public bool Update(Vehicle v)
        {
            if (Vehicles.Keys.Contains(v.Id))
            {
                Vehicles[v.Id] = v;
                return true;
            }

            return false;
        }

        public Vehicle Delete(int key)
        {
            Vehicle v;
            Vehicles.TryRemove(key, out v);
            return v;
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            return Vehicles.Values;
        }
    }
}
