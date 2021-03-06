﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{
    [ServiceContract(CallbackContract = typeof(IWFRPCallback), SessionMode = SessionMode.Required)]
    public interface IWFRP
    {
        [OperationContract(IsInitiating = true)]
        bool Initialize();

        [OperationContract(IsOneWay = true)]
        void Disconnect(Client client);

        [OperationContract(IsOneWay = true)]
        void Register(Client client);

        [OperationContract(IsOneWay = true)]
        void LogIn(Client client);

        [OperationContract(IsOneWay = true)]
        void StartSession(Client client, List<string> members);

        [OperationContract(IsOneWay = true)]
        void GetAllClients();

        [OperationContract(IsOneWay = true)]
        void EndSession(Client client);

        [OperationContract(IsOneWay = true)]
        void Send(Message msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(Message msg);

        [OperationContract(IsOneWay = true)]
        void AddMemberToSession(Client client, List<string> members);

        [OperationContract(IsOneWay = true)]
        void UpdateParchment(Client client, FileMessage fMsg);

        [OperationContract(IsOneWay = true)]
        void GetHero(string client);

        [OperationContract(IsOneWay = true)]
        void GetHeroEyeColor(string race);

        [OperationContract(IsOneWay = true)]
        void GetHeroOccupationByRace(string race);

        [OperationContract(IsOneWay = true)]
        void GetOccupationInfo(string occupation);

        [OperationContract(IsOneWay = true)]
        void AddHeroBasicInfo(HeroBasicInfo info);

        [OperationContract(IsOneWay = true)]
        void GetSkillsAndAbilities(HeroRaceAndOccupation info);

        [OperationContract(IsOneWay = true)]
        void AddHeroSkillsAndAbilities(HeroAbilitiesAndSkills AbsNSki);

        [OperationContract(IsOneWay = true)]
        void GetAbilityName(List<string> IDabilities);

        [OperationContract(IsOneWay = true)]
        void GetSkillName(List<string> IDskills);

        [OperationContract(IsOneWay = true)]
        void GetFullAbilityInfo(string abName);

        [OperationContract(IsOneWay = true)]
        void GetFullSkillInfo(string skName);

        [OperationContract(IsOneWay = true)]
        void AddStartStats(StartStats strSta);

        [OperationContract(IsOneWay = true)]
        void GetHeroChart(string Id_acc);

        [OperationContract(IsOneWay = true)]
        void GetRaceAbilityName(List<string> IDabilities);

        [OperationContract(IsOneWay = true)]
        void GetOccupationAbilityName(List<string> IDabilities);

        [OperationContract(IsOneWay = true)]
        void GetRaceSkillName(List<string> IDskills);

        [OperationContract(IsOneWay = true)]
        void GetOccupationSkillName(List<string> IDskills);

        [OperationContract(IsOneWay = true)]
        void CheckIfHeroCreated(string id_acc);

        [OperationContract(IsOneWay = true)]
        void GetHeroID(string name);

    }
}
