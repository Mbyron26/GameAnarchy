﻿using ColossalFramework;
using System;
using UnityEngine;

namespace GameAnarchy.Managers;

public partial class Manager {
    private static readonly ushort numBuildingChunks = 32;
    private static ushort nextBuildingChunk;

    public static Action[] FrameHander = new Action[] {
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

    private static void RemoveNoisePollution() {
        if (!Config.Instance.RemoveNoisePollution)
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.NoisePollution, -100000);
    }

    private static void RemoveGroundPollution() {
        if (!Config.Instance.RemoveGroundPollution) 
            return;
        Singleton<NaturalResourceManager>.instance.AddPollutionDisposeRate(10000);
    }

    private static void RemoveWaterPollution() {
        if (!Config.Instance.RemoveWaterPollution) 
            return;
        Singleton<TerrainManager>.instance.WaterSimulation.AddPollutionDisposeRate(10000);
    }

    private static void RemoveDeath() {
        if (!Config.Instance.RemoveDeath) 
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.DeathCare, 100000);
    }

    private static void RemoveGarbage() {
        if (!Config.Instance.RemoveGarbage) 
            return;
    }

    private static void RemoveCrime() {
        if (!Config.Instance.RemoveCrime) 
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.CrimeRate, -100000);
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.PoliceDepartment, 100000);
    }

    private static void RemoveFire() {
        if (!Config.Instance.MaximizeFireCoverage) 
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.FirewatchCoverage, 100000, Vector3.zero, 100000);
    }
    private static void MaximizeAttractiveness() {
        if (!Config.Instance.MaximizeAttractiveness) 
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.Attractiveness, 100000);
    }

    private static void MaximizeEntertainment() {
        if (!Config.Instance.MaximizeEntertainment) 
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.Entertainment, 100000);
    }

    private static void MaximizeLandValue() {
        if (!Config.Instance.MaximizeLandValue) 
            return;
        ImmaterialResourceManager.instance.AddResource(ImmaterialResourceManager.Resource.LandValue, 100000);
    }

    private static void MaximizeEducationCoverage() {
        if (!Config.Instance.MaximizeEducationCoverage) 
            return;
        Singleton<ImmaterialResourceManager>.instance.AddResource(ImmaterialResourceManager.Resource.EducationUniversity, 100000);
        Singleton<ImmaterialResourceManager>.instance.AddResource(ImmaterialResourceManager.Resource.EducationHighSchool, 100000);
        Singleton<ImmaterialResourceManager>.instance.AddResource(ImmaterialResourceManager.Resource.EducationElementary, 100000);
        DistrictManager dManager = Singleton<DistrictManager>.instance;
        District city = dManager.m_districts.m_buffer[0];
        city.m_productionData.m_tempEducation1Capacity += 1000000u;
        city.m_productionData.m_tempEducation2Capacity += 1000000u;
        city.m_productionData.m_tempEducation3Capacity += 1000000u;
    }

    private static void RemoveBuildingProblem() {
        BuildingManager bManager = Singleton<BuildingManager>.instance;
        var bBuffer = bManager.m_buildings.m_buffer;
        var chunkSize = bBuffer.Length / numBuildingChunks;
        var begin = (uint)(nextBuildingChunk * chunkSize);
        var end = (uint)Math.Min(begin + chunkSize, bBuffer.Length);
        nextBuildingChunk += 1;
        nextBuildingChunk %= numBuildingChunks;
        for (uint i = begin; i < end; i++) {
            if ((bBuffer[i].m_flags & Building.Flags.Created) == Building.Flags.None) continue;
            RemoveBuildingProblemBase(ref bBuffer[i]);
        }
    }

    private static void RemoveBuildingProblemBase(ref Building building) {
        if (Config.Instance.RemoveDeath) {
            CitizenManager cManager = Singleton<CitizenManager>.instance;
            var mBuffer = cManager.m_units.m_buffer;
            for (var cID = building.m_citizenUnits; cID != 0; cID = mBuffer[cID].m_nextUnit) {
                GetCitizenUnitDeath(ref mBuffer[cID]);
            }
        }
        if (Config.Instance.RemoveGarbage) 
            building.m_garbageBuffer = 0;
        if (Config.Instance.RemoveCrime) 
            building.m_crimeBuffer = 0;
    }

    private static void GetCitizenUnitDeath(ref CitizenUnit citizen) {
        if (!Config.Instance.RemoveDeath) 
            return;
        var cManager = Singleton<CitizenManager>.instance;
        var mBuffer = cManager.m_citizens.m_buffer;
        var ciBuffer = cManager.m_instances.m_buffer;
        for (int i = 0; i < 5; i++) {
            uint citizenID = citizen.GetCitizen(i);
            if (citizenID != 0u) {
                if (mBuffer[citizenID].Dead) {
                    ushort citizenInstanceId = mBuffer[citizenID].m_instance;
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
}

