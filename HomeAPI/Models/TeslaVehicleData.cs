﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeAPI.Models
{
    public class TeslaVehicleData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("vehicle_id")]
        public int VehicleId { get; set; }

        [JsonProperty("vin")]
        public string Vin { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("option_codes")]
        public string OptionCodes { get; set; }

        [JsonProperty("color")]
        public object Color { get; set; }

        [JsonProperty("tokens")]
        public IList<string> Tokens { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("in_service")]
        public bool InService { get; set; }

        [JsonProperty("id_s")]
        public string IdS { get; set; }

        [JsonProperty("calendar_enabled")]
        public bool CalendarEnabled { get; set; }

        [JsonProperty("api_version")]
        public int ApiVersion { get; set; }

        [JsonProperty("backseat_token")]
        public object BackseatToken { get; set; }

        [JsonProperty("backseat_token_updated_at")]
        public object BackseatTokenUpdatedAt { get; set; }

        [JsonProperty("charge_state")]
        public ChargeState ChargeState { get; set; }

        [JsonProperty("climate_state")]
        public ClimateState ClimateState { get; set; }

        [JsonProperty("drive_state")]
        public DriveState DriveState { get; set; }

        [JsonProperty("gui_settings")]
        public GuiSettings GuiSettings { get; set; }

        [JsonProperty("vehicle_config")]
        public VehicleConfig VehicleConfig { get; set; }

        [JsonProperty("vehicle_state")]
        public VehicleState VehicleState { get; set; }
    }

    public class ChargeState
    {

        [JsonProperty("battery_heater_on")]
        public bool BatteryHeaterOn { get; set; }

        [JsonProperty("battery_level")]
        public int BatteryLevel { get; set; }

        [JsonProperty("battery_range")]
        public double BatteryRange { get; set; }

        [JsonProperty("charge_current_request")]
        public int ChargeCurrentRequest { get; set; }

        [JsonProperty("charge_current_request_max")]
        public int ChargeCurrentRequestMax { get; set; }

        [JsonProperty("charge_enable_request")]
        public bool ChargeEnableRequest { get; set; }

        [JsonProperty("charge_energy_added")]
        public int ChargeEnergyAdded { get; set; }

        [JsonProperty("charge_limit_soc")]
        public int ChargeLimitSoc { get; set; }

        [JsonProperty("charge_limit_soc_max")]
        public int ChargeLimitSocMax { get; set; }

        [JsonProperty("charge_limit_soc_min")]
        public int ChargeLimitSocMin { get; set; }

        [JsonProperty("charge_limit_soc_std")]
        public int ChargeLimitSocStd { get; set; }

        [JsonProperty("charge_miles_added_ideal")]
        public int ChargeMilesAddedIdeal { get; set; }

        [JsonProperty("charge_miles_added_rated")]
        public int ChargeMilesAddedRated { get; set; }

        [JsonProperty("charge_port_cold_weather_mode")]
        public bool ChargePortColdWeatherMode { get; set; }

        [JsonProperty("charge_port_door_open")]
        public bool ChargePortDoorOpen { get; set; }

        [JsonProperty("charge_port_latch")]
        public string ChargePortLatch { get; set; }

        [JsonProperty("charge_rate")]
        public int ChargeRate { get; set; }

        [JsonProperty("charge_to_max_range")]
        public bool ChargeToMaxRange { get; set; }

        [JsonProperty("charger_actual_current")]
        public int ChargerActualCurrent { get; set; }

        [JsonProperty("charger_phases")]
        public object ChargerPhases { get; set; }

        [JsonProperty("charger_pilot_current")]
        public int ChargerPilotCurrent { get; set; }

        [JsonProperty("charger_power")]
        public int ChargerPower { get; set; }

        [JsonProperty("charger_voltage")]
        public int ChargerVoltage { get; set; }

        [JsonProperty("charging_state")]
        public string ChargingState { get; set; }

        [JsonProperty("conn_charge_cable")]
        public string ConnChargeCable { get; set; }

        [JsonProperty("est_battery_range")]
        public int EstBatteryRange { get; set; }

        [JsonProperty("fast_charger_brand")]
        public string FastChargerBrand { get; set; }

        [JsonProperty("fast_charger_present")]
        public bool FastChargerPresent { get; set; }

        [JsonProperty("fast_charger_type")]
        public string FastChargerType { get; set; }

        [JsonProperty("ideal_battery_range")]
        public double IdealBatteryRange { get; set; }

        [JsonProperty("managed_charging_active")]
        public bool ManagedChargingActive { get; set; }

        [JsonProperty("managed_charging_start_time")]
        public object ManagedChargingStartTime { get; set; }

        [JsonProperty("managed_charging_user_canceled")]
        public bool ManagedChargingUserCanceled { get; set; }

        [JsonProperty("max_range_charge_counter")]
        public int MaxRangeChargeCounter { get; set; }

        [JsonProperty("not_enough_power_to_heat")]
        public object NotEnoughPowerToHeat { get; set; }

        [JsonProperty("scheduled_charging_pending")]
        public bool ScheduledChargingPending { get; set; }

        [JsonProperty("scheduled_charging_start_time")]
        public object ScheduledChargingStartTime { get; set; }

        [JsonProperty("time_to_full_charge")]
        public int TimeToFullCharge { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("trip_charging")]
        public bool TripCharging { get; set; }

        [JsonProperty("usable_battery_level")]
        public int UsableBatteryLevel { get; set; }

        [JsonProperty("user_charge_enable_request")]
        public object UserChargeEnableRequest { get; set; }
    }

    public class ClimateState
    {

        [JsonProperty("battery_heater")]
        public bool BatteryHeater { get; set; }

        [JsonProperty("battery_heater_no_power")]
        public object BatteryHeaterNoPower { get; set; }

        [JsonProperty("climate_keeper_mode")]
        public string ClimateKeeperMode { get; set; }

        [JsonProperty("driver_temp_setting")]
        public double DriverTempSetting { get; set; }

        [JsonProperty("fan_status")]
        public int FanStatus { get; set; }

        [JsonProperty("inside_temp")]
        public double InsideTemp { get; set; }

        [JsonProperty("is_auto_conditioning_on")]
        public bool IsAutoConditioningOn { get; set; }

        [JsonProperty("is_climate_on")]
        public bool IsClimateOn { get; set; }

        [JsonProperty("is_front_defroster_on")]
        public bool IsFrontDefrosterOn { get; set; }

        [JsonProperty("is_preconditioning")]
        public bool IsPreconditioning { get; set; }

        [JsonProperty("is_rear_defroster_on")]
        public bool IsRearDefrosterOn { get; set; }

        [JsonProperty("left_temp_direction")]
        public int LeftTempDirection { get; set; }

        [JsonProperty("max_avail_temp")]
        public int MaxAvailTemp { get; set; }

        [JsonProperty("min_avail_temp")]
        public int MinAvailTemp { get; set; }

        [JsonProperty("outside_temp")]
        public int OutsideTemp { get; set; }

        [JsonProperty("passenger_temp_setting")]
        public double PassengerTempSetting { get; set; }

        [JsonProperty("remote_heater_control_enabled")]
        public bool RemoteHeaterControlEnabled { get; set; }

        [JsonProperty("right_temp_direction")]
        public int RightTempDirection { get; set; }

        [JsonProperty("seat_heater_left")]
        public int SeatHeaterLeft { get; set; }

        [JsonProperty("seat_heater_right")]
        public int SeatHeaterRight { get; set; }

        [JsonProperty("side_mirror_heaters")]
        public bool SideMirrorHeaters { get; set; }

        [JsonProperty("smart_preconditioning")]
        public bool SmartPreconditioning { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("wiper_blade_heater")]
        public bool WiperBladeHeater { get; set; }
    }

    public class DriveState
    {

        [JsonProperty("gps_as_of")]
        public int GpsAsOf { get; set; }

        [JsonProperty("heading")]
        public int Heading { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("native_latitude")]
        public double NativeLatitude { get; set; }

        [JsonProperty("native_location_supported")]
        public int NativeLocationSupported { get; set; }

        [JsonProperty("native_longitude")]
        public double NativeLongitude { get; set; }

        [JsonProperty("native_type")]
        public string NativeType { get; set; }

        [JsonProperty("power")]
        public int Power { get; set; }

        [JsonProperty("shift_state")]
        public object ShiftState { get; set; }

        [JsonProperty("speed")]
        public object Speed { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public class GuiSettings
    {

        [JsonProperty("gui_24_hour_time")]
        public bool Gui24HourTime { get; set; }

        [JsonProperty("gui_charge_rate_units")]
        public string GuiChargeRateUnits { get; set; }

        [JsonProperty("gui_distance_units")]
        public string GuiDistanceUnits { get; set; }

        [JsonProperty("gui_range_display")]
        public string GuiRangeDisplay { get; set; }

        [JsonProperty("gui_temperature_units")]
        public string GuiTemperatureUnits { get; set; }

        [JsonProperty("show_range_units")]
        public bool ShowRangeUnits { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public class VehicleConfig
    {

        [JsonProperty("can_accept_navigation_requests")]
        public bool CanAcceptNavigationRequests { get; set; }

        [JsonProperty("can_actuate_trunks")]
        public bool CanActuateTrunks { get; set; }

        [JsonProperty("car_special_type")]
        public string CarSpecialType { get; set; }

        [JsonProperty("car_type")]
        public string CarType { get; set; }

        [JsonProperty("charge_port_type")]
        public string ChargePortType { get; set; }

        [JsonProperty("eu_vehicle")]
        public bool EuVehicle { get; set; }

        [JsonProperty("exterior_color")]
        public string ExteriorColor { get; set; }

        [JsonProperty("has_air_suspension")]
        public bool HasAirSuspension { get; set; }

        [JsonProperty("has_ludicrous_mode")]
        public bool HasLudicrousMode { get; set; }

        [JsonProperty("key_version")]
        public int KeyVersion { get; set; }

        [JsonProperty("motorized_charge_port")]
        public bool MotorizedChargePort { get; set; }

        [JsonProperty("plg")]
        public object Plg { get; set; }

        [JsonProperty("rear_seat_heaters")]
        public int RearSeatHeaters { get; set; }

        [JsonProperty("rear_seat_type")]
        public object RearSeatType { get; set; }

        [JsonProperty("rhd")]
        public bool Rhd { get; set; }

        [JsonProperty("roof_color")]
        public string RoofColor { get; set; }

        [JsonProperty("seat_type")]
        public object SeatType { get; set; }

        [JsonProperty("spoiler_type")]
        public string SpoilerType { get; set; }

        [JsonProperty("sun_roof_installed")]
        public object SunRoofInstalled { get; set; }

        [JsonProperty("third_row_seats")]
        public string ThirdRowSeats { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("use_range_badging")]
        public bool UseRangeBadging { get; set; }

        [JsonProperty("wheel_type")]
        public string WheelType { get; set; }
    }

    public class MediaState
    {

        [JsonProperty("remote_control_enabled")]
        public bool RemoteControlEnabled { get; set; }
    }

    public class SoftwareUpdate
    {

        [JsonProperty("expected_duration_sec")]
        public int ExpectedDurationSec { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class SpeedLimitMode
    {

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("current_limit_mph")]
        public int CurrentLimitMph { get; set; }

        [JsonProperty("max_limit_mph")]
        public int MaxLimitMph { get; set; }

        [JsonProperty("min_limit_mph")]
        public int MinLimitMph { get; set; }

        [JsonProperty("pin_code_set")]
        public bool PinCodeSet { get; set; }
    }

    public class VehicleState
    {

        [JsonProperty("api_version")]
        public int ApiVersion { get; set; }

        [JsonProperty("autopark_state_v3")]
        public string AutoparkStateV3 { get; set; }

        [JsonProperty("calendar_supported")]
        public bool CalendarSupported { get; set; }

        [JsonProperty("car_version")]
        public string CarVersion { get; set; }

        [JsonProperty("center_display_state")]
        public int CenterDisplayState { get; set; }

        [JsonProperty("df")]
        public int Df { get; set; }

        [JsonProperty("dr")]
        public int Dr { get; set; }

        [JsonProperty("ft")]
        public int Ft { get; set; }

        [JsonProperty("is_user_present")]
        public bool IsUserPresent { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("media_state")]
        public MediaState MediaState { get; set; }

        [JsonProperty("notifications_supported")]
        public bool NotificationsSupported { get; set; }

        [JsonProperty("odometer")]
        public double Odometer { get; set; }

        [JsonProperty("parsed_calendar_supported")]
        public bool ParsedCalendarSupported { get; set; }

        [JsonProperty("pf")]
        public int Pf { get; set; }

        [JsonProperty("pr")]
        public int Pr { get; set; }

        [JsonProperty("remote_start")]
        public bool RemoteStart { get; set; }

        [JsonProperty("remote_start_enabled")]
        public bool RemoteStartEnabled { get; set; }

        [JsonProperty("remote_start_supported")]
        public bool RemoteStartSupported { get; set; }

        [JsonProperty("rt")]
        public int Rt { get; set; }

        [JsonProperty("sentry_mode")]
        public bool SentryMode { get; set; }

        [JsonProperty("sentry_mode_available")]
        public bool SentryModeAvailable { get; set; }

        [JsonProperty("software_update")]
        public SoftwareUpdate SoftwareUpdate { get; set; }

        [JsonProperty("speed_limit_mode")]
        public SpeedLimitMode SpeedLimitMode { get; set; }

        [JsonProperty("sun_roof_percent_open")]
        public object SunRoofPercentOpen { get; set; }

        [JsonProperty("sun_roof_state")]
        public string SunRoofState { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("valet_mode")]
        public bool ValetMode { get; set; }

        [JsonProperty("valet_pin_needed")]
        public bool ValetPinNeeded { get; set; }

        [JsonProperty("vehicle_name")]
        public string VehicleName { get; set; }
    }
}
