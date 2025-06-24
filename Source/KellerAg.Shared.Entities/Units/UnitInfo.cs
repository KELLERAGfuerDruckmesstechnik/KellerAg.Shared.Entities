using System;
using System.Data;
using System.Linq;

namespace KellerAg.Shared.Entities.Units;

public class UnitInfo : IEquatable<UnitInfo>
{
    private string _fullName;
    private string _shortName;
    private double _factor;
    private double _offset;
    private UnitType _unitType;

    /// <summary>
    /// Gets back a list of all valid units. The first unit is the default unit.
    /// </summary>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static UnitInfo[] GetUnits(UnitType unitType)
    {
        UnitInfo[] unitInfos;
        switch (unitType)
        {
            case UnitType.Pressure:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("bar", "bar", UnitType.Pressure, 1, 0),
                        new UnitInfo("millibar", "mbar", UnitType.Pressure, 1000, 0),
                        new UnitInfo("pound-force per square inch", "PSI", UnitType.Pressure, 14.5038, 0),
                        new UnitInfo("Megapascal", "MPa", UnitType.Pressure, 0.1, 0),
                        new UnitInfo("kilopascal", "kPa", UnitType.Pressure, 100, 0),
                        new UnitInfo("pascal", "Pa", UnitType.Pressure, 100000, 0)
                    };
                break;
            case UnitType.Length:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter", "m", UnitType.Length, 1, 0),
                        new UnitInfo("centimeter", "cm", UnitType.Length, 100, 0),
                        new UnitInfo("inch", "inch", UnitType.Length, 39.3701, 0),
                        new UnitInfo("feet", "ft", UnitType.Length, 3.28084, 0)
                    };
                break;
            case UnitType.Temperature:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("celsius", "°C", UnitType.Temperature, 1, 0),
                        new UnitInfo("fahrenheit", "°F", UnitType.Temperature, 1.8, 32)
                    };
                break;
            case UnitType.Conductivity:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("millisiemens/centimeter", "mS/cm", UnitType.Conductivity, 1, 0),
                        new UnitInfo("microsiemens/centimeter", "uS/cm", UnitType.Conductivity, 1000, 0)
                    };
                break;
            case UnitType.Voltage:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("volt", "V", UnitType.Voltage, 1, 0),
                    };
                break;
            case UnitType.Density:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("kilogram/meter³", "kg/m³", UnitType.Density, 1, 0),
                    };
                break;
            case UnitType.Acceleration:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter/second²", "m/s²", UnitType.Acceleration, 1, 0),
                    };
                break;
            case UnitType.Angle:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("degrees", "°", UnitType.Angle, 1, 0),
                    };
                break;
            case UnitType.Flow:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter³/second", "m³/s", UnitType.Flow, 1, 0),
                        new UnitInfo("liters/second", "l/s", UnitType.Flow, 1000, 0),
                    };
                break;
            case UnitType.Volume:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter³", "m³", UnitType.Volume, 1, 0),
                        new UnitInfo("liter", "l", UnitType.Volume, 1000, 0),
                    };
                break;
            case UnitType.Force:
                unitInfos = new[]
                {
                        //Be aware that the first unit is the default unit
                        new UnitInfo("newton", "N", UnitType.Force, 1, 0),
                        new UnitInfo("kilonewton", "kN", UnitType.Force, 1000, 0),
                    };
                break;
            case UnitType.Area:
                unitInfos = new[]
                {                        
                        //Be aware that the first unit is the default unit
                        new UnitInfo("meter²", "m²", UnitType.Area, 1, 0),
                        new UnitInfo("centimeter²", "cm²", UnitType.Area, 10000, 0),
                        new UnitInfo("inch²", "inch²", UnitType.Area, 1550, 0),
                        new UnitInfo("feet²", "ft²", UnitType.Area, 10.7639, 0)
                    };
                break;
            case UnitType.Unitless:
                unitInfos = new[]
                {                        
                        //Be aware that the first unit is the default unit
                        new UnitInfo("", "", UnitType.Unitless, 1, 0),
                    };
                break;
            case UnitType.Unknown:
            default:
                unitInfos = Array.Empty<UnitInfo>();
                break;
        }
        return unitInfos;
    }

    /// <summary>
    /// Returns the current unit for all unit types.
    /// </summary>
    /// <returns></returns>
    public static UnitInfo[] CurrentUnits()
    {
        var unitTypes = GetAllUnitTypes();
        var unitInfos = new UnitInfo[unitTypes.Length];
        var i = 0;
        foreach (var unit in unitTypes)
        {
            unitInfos[i] = CurrentUnit(unit);
            i++;
        }

        return unitInfos;
    }

    /// <summary>
    /// Returns the base unit for all unit types.
    /// </summary>
    /// <returns></returns>
    public static UnitInfo[] BaseUnits()
    {
        var unitTypes = GetAllUnitTypes();
        var unitInfos = new UnitInfo[unitTypes.Length];
        var i = 0;
        foreach (var unit in unitTypes)
        {
            unitInfos[i] = BaseUnit(unit);
            i++;
        }

        return unitInfos;
    }

    /// <summary>
    /// Return the base unit for the given unit type.
    /// </summary>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static UnitInfo BaseUnit(UnitType unitType)
    {
        return GetUnits(unitType).FirstOrDefault();
    }

    /// <summary>
    /// Returns the current unit for the given unit type.
    /// </summary>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static UnitInfo CurrentUnit(UnitType unitType)
    {
        var unit = unitType switch
        {
            UnitType.Pressure => CurrentPressureUnit ?? GetUnits(UnitType.Pressure).FirstOrDefault(),
            UnitType.Length => CurrentLengthUnit ?? GetUnits(UnitType.Length).FirstOrDefault(),
            UnitType.Temperature => CurrentTemperatureUnit ?? GetUnits(UnitType.Temperature).FirstOrDefault(),
            UnitType.Conductivity => CurrentConductivityUnit ?? GetUnits(UnitType.Conductivity).FirstOrDefault(),
            UnitType.Voltage => CurrentVoltageUnit ?? GetUnits(UnitType.Voltage).FirstOrDefault(),
            UnitType.Volume => CurrentVolumeUnit ?? GetUnits(UnitType.Volume).FirstOrDefault(),
            UnitType.Density => CurrentDensityUnit ?? GetUnits(UnitType.Density).FirstOrDefault(),
            UnitType.Acceleration => CurrentAccelerationUnit ?? GetUnits(UnitType.Acceleration).FirstOrDefault(),
            UnitType.Angle => CurrentAngleUnit ?? GetUnits(UnitType.Angle).FirstOrDefault(),
            UnitType.Flow => CurrentFlowUnit ?? GetUnits(UnitType.Flow).FirstOrDefault(),
            UnitType.Force => CurrentForceUnit ?? GetUnits(UnitType.Force).FirstOrDefault(),
            UnitType.Area => CurrentAreaUnit ?? GetUnits(UnitType.Area).FirstOrDefault(),
            UnitType.Unitless => CurrentUnitlessUnit ?? GetUnits(UnitType.Unitless).FirstOrDefault(),
            UnitType.Unknown => new UnitInfo("", "", UnitType.Unknown, 1, 0),
            _ => new UnitInfo()
        };
        return unit;
    }

    /// <summary>
    /// Sets the current unit for the given unit type.
    /// </summary>
    /// <param name="unit"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void SetCurrentUnit(UnitInfo unit)
    {
        switch (unit.UnitType)
        {
            case UnitType.Pressure:
                CurrentPressureUnit = unit;
                break;
            case UnitType.Length:
                CurrentLengthUnit = unit;
                break;
            case UnitType.Temperature:
                CurrentTemperatureUnit = unit;
                break;
            case UnitType.Conductivity:
                CurrentConductivityUnit = unit;
                break;
            case UnitType.Voltage:
                CurrentVoltageUnit = unit;
                break;
            case UnitType.Volume:
                CurrentVolumeUnit = unit;
                break;
            case UnitType.Density:
                CurrentDensityUnit = unit;
                break;
            case UnitType.Acceleration:
                CurrentAccelerationUnit = unit;
                break;
            case UnitType.Angle:
                CurrentAngleUnit = unit;
                break;
            case UnitType.Flow:
                CurrentFlowUnit = unit;
                break;
            case UnitType.Force:
                CurrentForceUnit = unit;
                break;
            case UnitType.Area:
                CurrentAreaUnit = unit;
                break;
            case UnitType.Unitless:
                CurrentUnitlessUnit = unit;
                break;
            case UnitType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Converts a number from the current unit to the base unit.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public double ToBase(double number)
    {
        return (number - Offset) / Factor;
    }

    /// <summary>
    /// Converts a nullable number from the current unit to the base unit.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public double? ToBase(double? number)
    {
        return (number - Offset) / Factor;
    }

    /// <summary>
    /// Converts a number from the base unit to the current unit.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public double FromBase(double number)
    {
        return (Factor * number) + Offset;
    }

    /// <summary>
    /// Converts a nullable number from the base unit to the current unit.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public double? FromBase(double? number)
    {
        return Factor * number + Offset;
    }

    /// <summary>
    /// Converts a number from the base unit to the current unit for the given unit type.
    /// </summary>
    /// <param name="numberInBaseUnit"></param>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static double BaseToCurrent(double numberInBaseUnit, UnitType unitType)
    {
        return CurrentUnit(unitType).FromBase(numberInBaseUnit);
    }

    /// <summary>
    /// Converts a number from the current unit to the base unit for the given unit type.
    /// </summary>
    /// <param name="numberInCurrentUnit"></param>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static double CurrentToBase(double numberInCurrentUnit, UnitType unitType)
    {
        return CurrentUnit(unitType).ToBase(numberInCurrentUnit);
    }

    private static UnitType[] GetAllUnitTypes()
    {
        return Enum.GetValues(typeof(UnitType)).Cast<UnitType>().ToArray();
    }

    private static UnitInfo CurrentPressureUnit { get; set; }
    private static UnitInfo CurrentTemperatureUnit { get; set; }
    private static UnitInfo CurrentLengthUnit { get; set; }
    private static UnitInfo CurrentConductivityUnit { get; set; }
    private static UnitInfo CurrentVoltageUnit { get; set; }
    private static UnitInfo CurrentVolumeUnit { get; set; }
    private static UnitInfo CurrentDensityUnit { get; set; }
    private static UnitInfo CurrentAccelerationUnit { get; set; }
    private static UnitInfo CurrentAngleUnit { get; set; }
    private static UnitInfo CurrentFlowUnit { get; set; }
    private static UnitInfo CurrentForceUnit { get; set; }
    private static UnitInfo CurrentAreaUnit { get; set; }
    private static UnitInfo CurrentUnitlessUnit { get; set; }

    public UnitInfo()
    {
    }

    public UnitInfo(string fullName, string shortName, UnitType unitType, double factor, double offset, bool isReadonly = true)
    {
        _fullName = fullName;
        _shortName = shortName;
        _unitType = unitType;
        _factor = factor;
        _offset = offset;
        IsReadonly = isReadonly;
    }

    /// <summary>
    /// Full name of the unit, e.g. "bar", "millibar", "pound-force per square inch", etc.
    /// </summary>
    public string FullName
    {
        get => _fullName;
        set
        {
            ReadOnlyCheck();
            _fullName = value;
        }
    }

    /// <summary>
    /// Short name of the unit, e.g. "bar", "mbar", "PSI", etc.
    /// </summary>
    public string ShortName
    {
        get => _shortName;
        set
        {

            ReadOnlyCheck();
            _shortName = value;
        }
    }

    /// <summary>
    /// Conversion factor from the base unit.
    /// </summary>
    public double Factor
    {
        get => _factor;
        set
        {

            ReadOnlyCheck();
            _factor = value;
        }
    }

    /// <summary>
    /// Conversion offset shifting the zero point from the base unit.
    /// </summary>
    public double Offset
    {
        get => _offset;
        set
        {
            ReadOnlyCheck();
            _offset = value;
        }
    }

    /// <summary>
    /// Unit type, e.g. Pressure, Length, Temperature, etc.
    /// </summary>
    public UnitType UnitType
    {
        get => _unitType;
        set
        {

            ReadOnlyCheck();
            _unitType = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsReadonly { get; }

    private void ReadOnlyCheck()
    {
        if (IsReadonly)
        {
            throw new ReadOnlyException($"This {typeof(UnitInfo)} was initialized as readonly");
        }
    }

    /// <inheritdoc />
    public bool Equals(UnitInfo other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _fullName == other._fullName && _shortName == other._shortName && _factor.Equals(other._factor) && _offset.Equals(other._offset) && _unitType == other._unitType && IsReadonly == other.IsReadonly;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((UnitInfo)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + _fullName.GetHashCode();
            hash = hash * 23 + _shortName.GetHashCode();
            hash = hash * 23 + _factor.GetHashCode();
            hash = hash * 23 + _offset.GetHashCode();
            hash = hash * 23 + _unitType.GetHashCode();
            hash = hash * 23 + IsReadonly.GetHashCode();
            return hash;
        }
        //Only since netstandard2.1
        //return HashCode.Combine(_fullName, _shortName, _factor, _offset, (int) _unitType, IsReadonly);
    }
}
