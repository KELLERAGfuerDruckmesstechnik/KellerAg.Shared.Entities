using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KellerAg.Shared.Entities.Channel;

namespace KellerAg.Shared.Entities.Device;
    
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class DeviceTypes : Dictionary<DeviceType, string>
{
    //Pictures should be in \Resources\Devices\ preverably as jpg (faster than png) and with a resolution of 318x500 or same factor
    //Except Converter: They have 467x254
    public static DeviceTypes PictureFileName { get; } = new()
        {
            { DeviceType.Unknown         , "dcx_22aaCombo.jpg"},
            { DeviceType.Castello        , "Castello.jpg"},
            { DeviceType.DCX22           , "dcx_22.jpg"},
            { DeviceType.DCX18ECO        , "dcx_18.jpg"},
            { DeviceType.DCX22AA         , "dcx_22aaCombo.jpg"},
            { DeviceType.DCX_CTD         , "dcx_22aaCombo.jpg"},
            { DeviceType.dV_2            , "dV-2.jpg"},
            { DeviceType.dV_2Cool        , "dV-2.jpg"},
            { DeviceType.dV_2PP          , "dV-22_PP.jpg"},
            { DeviceType.dV_2PS          , "dV-2_PS.jpg"},
            { DeviceType.dV_2_Radtke     , "dV-2.jpg"},
            { DeviceType.ConverterK114   , "k114.jpg"},
            { DeviceType.ConverterK114_M , "k114.jpg"},
            { DeviceType.ConverterK114_BT, "k114_bt.jpg"},
            { DeviceType.LEO1_2          , "LEO2.jpg"},
            { DeviceType.LeoVolvo        , "LEO1.jpg"},
            { DeviceType.LeoIsler        , "LEO1.jpg"},
            { DeviceType.LeoGuehring     , "LEO1.jpg"},
            { DeviceType.LEO1x           , "LEO1.jpg"},
            { DeviceType.LEO3            , "LEO3.jpg"},
            { DeviceType.ECO1            , "ECO1.jpg"},
            { DeviceType.Leo5            , "LEO5.jpg"},
            { DeviceType.LeoRecord       , "LEO_Record.jpg"},
            { DeviceType.Lex1            , "LEX1.jpg"},
            { DeviceType.S30X            , "36w.jpg"},
            { DeviceType.S30X2           , "36xiw.jpg"},
            { DeviceType.S30X2_Cond      , "36xiw_ctd.jpg"},
            { DeviceType.ADT1            , "ADT1.jpg"},
            { DeviceType.ADT1_cellular   , "ADT1.jpg"},
            { DeviceType.ARC1            , "ARC1.jpg"},
            { DeviceType.ARC1_lora       , "ARC1.jpg"},
            { DeviceType.GSM1            , "ARC1.jpg"},
            { DeviceType.GSM2            , "ARC1.jpg"},
            { DeviceType.GSM3            , "ARC1.jpg"},
            { DeviceType.Bt_Transmitter  , "BT_Transmitter.jpg"},
            { DeviceType.LEO_Ultimate    , "LEO_Ultimate.jpg"},
        };


    private static DeviceTypes ProductUrl_EN { get; } = new()
        {
            { DeviceType.Unknown         , "https://keller-pressure.com/en/products"},
            { DeviceType.Castello        , "https://keller-pressure.com/en/products"},
            { DeviceType.DCX22           , "https://keller-pressure.com/en/products/data-loggers/level-loggers/dcx-22"},
            { DeviceType.DCX18ECO        , "https://keller-pressure.com/en/products"},
            { DeviceType.DCX22AA         , "https://keller-pressure.com/en/products/data-loggers/level-loggers/dcx-22aa"},
            { DeviceType.DCX_CTD         , "https://keller-pressure.com/en/products/data-loggers/multi-parameter-loggers/dcx-22aa-ctd"},
            { DeviceType.dV_2            , "https://keller-pressure.com/en/products"},
            { DeviceType.dV_2Cool        , "https://keller-pressure.com/en/products"},
            { DeviceType.dV_2PP          , "https://keller-pressure.com/en/products"},
            { DeviceType.dV_2PS          , "https://keller-pressure.com/en/products"},
            { DeviceType.dV_2_Radtke     , "https://keller-pressure.com/en/products/digital-pressure-gauges"},
            { DeviceType.ConverterK114   , "https://keller-pressure.com/en/products/software-accessories"},
            { DeviceType.ConverterK114_M , "https://keller-pressure.com/en/products/software-accessories"},
            { DeviceType.ConverterK114_BT, "https://keller-pressure.com/en/products/software-accessories"},
            { DeviceType.LEO1_2          , "https://keller-pressure.com/en/products/digital-pressure-gauges"},
            { DeviceType.LeoVolvo        , "https://keller-pressure.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoIsler        , "https://keller-pressure.com/en/products/digital-pressure-gauges"},
            { DeviceType.LeoGuehring     , "https://keller-pressure.com/en/products/digital-pressure-gauges"},
            { DeviceType.LEO1x           , "https://keller-pressure.com/en/products/digital-pressure-gauges"},
            { DeviceType.LEO3            , "https://keller-pressure.com/en/products/digital-pressure-gauges/digital-pressure-gauges/leo3"},
            { DeviceType.ECO1            , "https://keller-pressure.com/en/products/digital-pressure-gauges/intrinsically-safe-digital-pressure-gauges/eco2-ei"},
            { DeviceType.Leo5            , "https://keller-pressure.com/en/products/digital-pressure-gauges/digital-pressure-gauges/leo5"},
            { DeviceType.LeoRecord       , "https://keller-pressure.com/en/products/digital-pressure-gauges/digital-pressure-gauges/leo-record"},
            { DeviceType.Lex1            , "https://keller-pressure.com/en/products/digital-pressure-gauges/digital-pressure-gauges/lex1"},
            { DeviceType.S30X            , "https://keller-pressure.com/en/products/level-probes"},
            { DeviceType.S30X2           , "https://keller-pressure.com/en/products/level-probes"},
            { DeviceType.S30X2_Cond      , "https://keller-pressure.com/en/products/level-probes/multi-parameter-probes/series-36xiw-ctd"},
            { DeviceType.ADT1            , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ADT1_cellular   , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ARC1            , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.ARC1_lora       , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM1            , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM2            , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM3            , "https://keller-pressure.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.Bt_Transmitter  , "https://keller-pressure.com/en/products"},
            { DeviceType.LEO_Ultimate    , "https://keller-pressure.com/en/products"},
        };

    /// <summary>
    ///  Returns the product URL for the device types in English.
    /// </summary>
    /// <returns></returns>
    public static DeviceTypes ProductUrl()
    {
        return ProductUrl_EN;
    }

    /// <summary>
    /// Returns true if the device is a converter (K114, K114 BT, K114 M)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsConverter(DeviceType type) => type == DeviceType.ConverterK114 ||
                                                       type == DeviceType.ConverterK114_BT ||
                                                       type == DeviceType.ConverterK114_M;

    /// <summary>
    /// Returns true if the device is a LoRa or Cellular device
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsIotDevice(DeviceType type) => IsLoRaDevice(type) ||
                                                       IsCellularDevice(type);

    /// <summary>
    /// Returns true if the device is a logger device (DCX, LEO5, LEO Record, ARC1, ADT1, LEO Ultimate)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsLoggerDevice(DeviceType type) => type == DeviceType.LeoRecord ||
                                                       type == DeviceType.Leo5 ||
                                                       type == DeviceType.DCX18ECO ||
                                                       type == DeviceType.DCX22 ||
                                                       type == DeviceType.DCX22AA ||
                                                       type == DeviceType.DCX_CTD ||
                                                       type == DeviceType.LEO_Ultimate ||
                                                       type == DeviceType.Bt_Transmitter ||
                                                       IsIotDevice(type);

    /// <summary>
    /// Returns true if the device is a LoRa device (ADT1, ARC1_LR)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsLoRaDevice(DeviceType type) => type == DeviceType.ARC1_lora ||
                                                        type == DeviceType.ADT1;

    /// <summary>
    /// Returns true if the device is a Cellular device (ADT1_cellular, GSM1, GSM2, GSM3, ARC1)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsCellularDevice(DeviceType type) => type == DeviceType.GSM1 ||
                                                        type == DeviceType.GSM2 ||
                                                        type == DeviceType.GSM3 ||
                                                        type == DeviceType.ARC1 ||
                                                        type == DeviceType.ADT1_cellular;

    /// <summary>
    /// Returns true if the device is a CDT device
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public static bool IsCDTDevice(IDevice device)
    {
        bool? cdtChannelsEnabled = device?.DeviceInfo?.ChannelTypes?.Any(
                                                        channel => channel == ChannelType.ConductivityRaw ||
                                                        channel == ChannelType.ConductivityTc ||
                                                        channel == ChannelType.ConductivityTc_2 ||
                                                        channel == ChannelType.ConductivityTc_3 ||
                                                        channel == ChannelType.T_Conductivity ||
                                                        channel == ChannelType.T_Conductivity_2 ||
                                                        channel == ChannelType.T_Conductivity_3);
        // ABB AquaMaster would probably also need a long delay

        return cdtChannelsEnabled.HasValue && cdtChannelsEnabled.Value;
    }

    /// <summary>
    /// Determines whether the specified device type supports the zero function.
    /// </summary>
    /// <remarks>The zero function is available only for specific device types, primarily
    /// manometers.</remarks>
    /// <param name="type">The device type to check.</param>
    /// <returns><see langword="true"/> if the specified device type supports the zero function;  otherwise, <see
    /// langword="false"/>.</returns>
    public static bool HasZeroFunction(DeviceType type) => type == DeviceType.LeoRecord ||
                                                           type == DeviceType.Leo5 ||
                                                           type == DeviceType.LEO1_2 ||
                                                           type == DeviceType.LEO1x ||
                                                           type == DeviceType.LEO3 ||
                                                           type == DeviceType.LeoIsler ||
                                                           type == DeviceType.LeoGuehring ||
                                                           type == DeviceType.LeoVolvo ||
                                                           type == DeviceType.Lex1 ||
                                                           type == DeviceType.LEO_Ultimate;

    /// <summary>
    /// Determines the page size for a given device type.
    /// </summary>
    /// <param name="type">The type of the device for which the page size is being requested.</param>
    /// <returns>The page size of the device, in bytes. Returns 256 for <see cref="DeviceType.Bt_Transmitter"/>;  otherwise,
    /// returns 64.</returns>
    public static int DevicePageSize(DeviceType type) => type == DeviceType.Bt_Transmitter ? 256 : 64;
}