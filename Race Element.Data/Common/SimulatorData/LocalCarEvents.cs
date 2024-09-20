﻿namespace RaceElement.Data.Common.SimulatorData;

/// <summary>
/// Contains a collection of events related to changes in <see cref="SimDataProvider.LocalCar"/>.
/// </summary>
public sealed class LocalCarEvents
{
    internal LocalCarEvents() { }

    /// <summary>
    /// See <see cref="LocalCarData.CarModel"/> for live data.
    /// </summary>
    public readonly CarModelEvents CarModel = new();

    /// <summary>
    /// See <see cref="LocalCarData.Inputs"/> for live data.
    /// </summary>
    public readonly InputEvents Inputs = new();

    /// <summary>
    /// See <see cref="LocalCarData.Race"/> for live data.
    /// </summary>
    public readonly RaceEvents Race = new();


    #region Event Handlers

    /// <summary>
    /// Used to describe the changes between a previous and a next of this <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The <typeparamref name="T"/> that has changed</typeparam>
    /// <param name="Previous">Previous state</param>
    /// <param name="Next">Next state</param>
    public readonly record struct ChangeEvent<T>(T Previous, T Next);


    /// <see cref="LocalCarData.CarModel"/>
    public sealed class CarModelEvents
    {
        internal CarModelEvents() { }

        /// <summary>
        /// Kicks off when the Game Name of the car model is changed
        /// </summary>
        public event EventHandler<ChangeEvent<string>>? OnGameNameChanged;
        internal void GameNameChanged(ChangeEvent<string> gameNameChange) => OnGameNameChanged?.Invoke(this, gameNameChange);
    }

    /// <see cref="LocalCarData.Inputs"/>
    public sealed class InputEvents
    {
        internal InputEvents() { }
        /// <summary>
        /// Kicks off when the gear is changed
        /// </summary>
        public event EventHandler<ChangeEvent<int>>? OnGearChanged;
        internal void GearChanged(ChangeEvent<int> gearChangedEvent) => OnGearChanged?.Invoke(this, gearChangedEvent);

    }

    /// <see cref="LocalCarData.Race"/>
    public sealed class RaceEvents
    {
        internal RaceEvents() { }

        /// <summary>
        /// Kicks off when the driven lap count is changed.
        /// </summary>
        public event EventHandler<ChangeEvent<int>>? OnLapsCompletedChanged;
        internal void LapsDrivenChanged(ChangeEvent<int> lapsCompleted) => OnLapsCompletedChanged?.Invoke(this, lapsCompleted);

        /// <summary>
        /// Kicks of when the global position for the local car is changed.
        /// </summary>
        public event EventHandler<ChangeEvent<int>>? OnGlobalPositionChanged;
        internal void GlobalPositionChanged(ChangeEvent<int> globalPositionChangedEvent) => OnGlobalPositionChanged?.Invoke(this, globalPositionChangedEvent);

        /// <summary>
        /// Kicks of when the global position for the local car is changed.
        /// </summary>
        public event EventHandler<ChangeEvent<int>>? OnClassPositionChanged;
        internal void ClassPositionChanged(ChangeEvent<int> classPositionChangedEvent) => OnClassPositionChanged?.Invoke(this, classPositionChangedEvent);
    }

    #endregion

}
