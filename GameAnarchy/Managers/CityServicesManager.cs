using System;
using ColossalFramework;
using CSLModsCommon;
using CSLModsCommon.Manager;
using GameAnarchy.ModSettings;
using ICities;
using UnityEngine;

namespace GameAnarchy.Managers;

public class CityServicesManager : ManagerBase, ISimulation {
    private readonly ushort _numBuildingChunks = 32;
    private ushort _nextBuildingChunk;
    private Action[] _frameHandler;
    private byte _frameIndex;
    private ModSetting _modSetting;
    private IThreading _threading;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        _frameHandler = new[] {
            RemoveNoisePollution,
            RemoveGroundPollution,
            RemoveWaterPollution,
            RemoveDeath,
            RemoveGarbage,
            RemoveCrime,
            RemoveFire,
            MaximizeAttractiveness,
            MaximizeEntertainment,
            MaximizeLandValue,
            MaximizeEducationCoverage,
            RemoveBuildingProblem
        };
    }


    public void OnBindThreadingContext(IThreading threading) => _threading = threading;

    public void OnUnbindThreadingContext() { }
    public void OnThreadingUpdate(float realTimeDelta, float simulationTimeDelta) { }
    public void OnPreSimulationTick() { }
    public void OnPreSimulationFrame() { }

    public void OnPostSimulationFrame() {
        if (!(_threading.managers.loading is not null && _threading.managers.loading.loadingComplete && _threading.managers.loading.currentMode == AppMode.Game) || SimulationManager.instance.SimulationPaused)
            return;
        _frameIndex = (byte)(SimulationManager.instance.m_currentFrameIndex & 15);
        if (_frameHandler.Length <= _frameIndex)
            return;
        _frameHandler[_frameIndex].Invoke();
    }

    public void OnPostSimulationTick() { }

    private void RemoveNoisePollution() {
        if (!_modSetting.RemoveNoisePollution)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.NoisePollution, -100000);
    }

    private void RemoveGroundPollution() {
        if (!_modSetting.RemoveGroundPollution)
            return;
        Singleton<NaturalResourceManager>.instance.AddPollutionDisposeRate(10000);
    }

    private void RemoveWaterPollution() {
        if (!_modSetting.RemoveWaterPollution)
            return;
        Singleton<TerrainManager>.instance.WaterSimulation.AddPollutionDisposeRate(10000);
    }

    private void RemoveDeath() {
        if (!_modSetting.RemoveDeath)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.DeathCare, 100000);
    }

    private void RemoveGarbage() { }

    private void RemoveCrime() {
        if (!_modSetting.RemoveCrime)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.CrimeRate, -100000);
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.PoliceDepartment, 100000);
    }

    private void RemoveFire() {
        if (!_modSetting.MaximizeFireCoverage)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.FirewatchCoverage, 100000, Vector3.zero, 100000);
    }

    private void MaximizeAttractiveness() {
        if (!_modSetting.MaximizeAttractiveness)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.Attractiveness, 100000);
    }

    private void MaximizeEntertainment() {
        if (!_modSetting.MaximizeEntertainment)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.Entertainment, 100000);
    }

    private void MaximizeLandValue() {
        if (!_modSetting.MaximizeLandValue)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.LandValue, 100000);
    }

    private void MaximizeEducationCoverage() {
        if (!_modSetting.MaximizeEducationCoverage)
            return;
        Singleton<ImmaterialResourceManager>.instance.AddResource(ImmaterialResourceManager.Resource.EducationUniversity, 100000);
        Singleton<ImmaterialResourceManager>.instance.AddResource(ImmaterialResourceManager.Resource.EducationHighSchool, 100000);
        Singleton<ImmaterialResourceManager>.instance.AddResource(ImmaterialResourceManager.Resource.EducationElementary, 100000);
        var dManager = Singleton<DistrictManager>.instance;
        var city = dManager.m_districts.m_buffer[0];
        city.m_productionData.m_tempEducation1Capacity += 1000000u;
        city.m_productionData.m_tempEducation2Capacity += 1000000u;
        city.m_productionData.m_tempEducation3Capacity += 1000000u;
    }

    private void RemoveBuildingProblem() {
        var bManager = Singleton<BuildingManager>.instance;
        var bBuffer = bManager.m_buildings.m_buffer;
        var chunkSize = bBuffer.Length / _numBuildingChunks;
        var begin = (uint)(_nextBuildingChunk * chunkSize);
        var end = (uint)Math.Min(begin + chunkSize, bBuffer.Length);
        _nextBuildingChunk += 1;
        _nextBuildingChunk %= _numBuildingChunks;
        for (var i = begin; i < end; i++) {
            if ((bBuffer[i].m_flags & Building.Flags.Created) == Building.Flags.None) continue;
            RemoveBuildingProblemBase(ref bBuffer[i]);
        }
    }

    private void RemoveBuildingProblemBase(ref Building building) {
        if (_modSetting.RemoveDeath) {
            var cManager = Singleton<CitizenManager>.instance;
            var mBuffer = cManager.m_units.m_buffer;
            for (var cID = building.m_citizenUnits; cID != 0; cID = mBuffer[cID].m_nextUnit) GetCitizenUnitDeath(ref mBuffer[cID]);
        }

        if (_modSetting.RemoveGarbage)
            building.m_garbageBuffer = 0;
        if (_modSetting.RemoveCrime)
            building.m_crimeBuffer = 0;
    }

    private void GetCitizenUnitDeath(ref CitizenUnit citizen) {
        if (!_modSetting.RemoveDeath)
            return;
        var cManager = Singleton<CitizenManager>.instance;
        var mBuffer = cManager.m_citizens.m_buffer;
        var ciBuffer = cManager.m_instances.m_buffer;
        for (var i = 0; i < 5; i++) {
            var citizenID = citizen.GetCitizen(i);
            if (citizenID != 0u)
                if (mBuffer[citizenID].Dead) {
                    var citizenInstanceId = mBuffer[citizenID].m_instance;
                    if (citizenInstanceId != 0) {
                        mBuffer[citizenID].m_instance = 0;
                        ciBuffer[citizenInstanceId].m_citizen = 0u;
                        cManager.ReleaseCitizenInstance(citizenInstanceId);
                    }

                    var family = mBuffer[citizenID].m_family;
                    cManager.ReleaseCitizen(citizenID);
                    cManager.CreateCitizen(out _, 5, family, ref SimulationManager.instance.m_randomizer);
                }
        }
    }
}