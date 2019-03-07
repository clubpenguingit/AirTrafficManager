using System;
using System.Text;

namespace AirTrafficManager
{
    public class MonitoredAirspace
    {
        public MonitoredAirspace(int upperX, int lowerX, int upperY, int lowerY, int upperAltitude, int lowerAltitude)
        {
            UpperBoundX = upperX;
            LowerBoundX = lowerX;
            UpperBoundY = upperY;
            LowerBoundY = lowerY;
            UpperAltitudeBound = upperAltitude;
            LowerAltitudeBound = lowerAltitude;
        }

        private int _upperBoundX;
        private int _lowerBoundX;
        private int _upperBoundY;
        private int _lowerBoundY;
        private int _upperAltitudeBound;
        private int _lowerAltitudeBound;
        
        public int UpperBoundX
        {
            get { return _upperBoundX; }
            set
            {
                if ((value > 95000) || (value < 5000))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is out of bounds, range is 5000 to 95000 meters");
                }

                _upperBoundX = value;
            }
        }

        public int LowerBoundX
        {
            get { return _lowerBoundX;}
            set
            {
                if ((value > 95000) || (value < 5000))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is out of bounds, range is 5000 to 95000 meters");
                }

                _lowerBoundX = value;
            }
        }

        public int UpperBoundY
        {
            get { return _upperBoundY;}
            set
            {
                if ((value > 95000) || (value < 5000))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is out of bounds, range is 5000 to 95000 meters");
                }

                _upperBoundY = value;
            }
        }

        public int LowerBoundY
        {
            get { return _lowerBoundY;}
            set
            {
                if ((value > 95000) || (value < 5000))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is out of bounds, range is 5000 to 95000 meters");
                }

                _lowerBoundY = value;
            }
        }

        public int UpperAltitudeBound
        {
            get { return _upperAltitudeBound;}
            set
            {
                if ((value > 20000) || (value < 500))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is out of bounds, range is 500 to 20000 meters");
                }

                _upperAltitudeBound = value;
            }
        }

        public int LowerAltitudeBound
        {
            get { return _lowerAltitudeBound;}
            set
            {
                if ((value > 20000) || (value < 500))
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is out of bounds, range is 500 to 20000 meters");
                }

                _lowerAltitudeBound = value;
            }
        }

        public bool ValidateAirspace(Track needsValidation)
        {
           

            if ((needsValidation.XCoordinate > _upperBoundX) || (needsValidation.XCoordinate < _lowerBoundX))
            {
                return false;
            }

            else if ((needsValidation.YCoordinate > _upperBoundY) || (needsValidation.YCoordinate < _lowerBoundY))
            {
                return false;
            }

            else if ((needsValidation.Altitude > _upperAltitudeBound) || (needsValidation.Altitude < _lowerAltitudeBound))
            {
                return false;
            }

            else
            {
                return true;
            }
        }

    }      
}          