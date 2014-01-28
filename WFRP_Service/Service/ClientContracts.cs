using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{
    public interface IWFRPCallback
    {
        [OperationContract(IsOneWay = true)]
        void GetServerMessageStatus(ServerMessage msg);

        [OperationContract(IsOneWay = true)]
        void GetIdentity(Identity userID);

        [OperationContract(IsOneWay = true)]
        void SetClientList(List<string> clients);

        [OperationContract(IsOneWay = true)]
        void JoinedToSession(ServerMessage msg);

        [OperationContract(IsOneWay = true)]
        void SessionInitSettings(Session session);

        [OperationContract(IsOneWay = true)]
        void SetSessionList(List<string> clients, Message msg);

        [OperationContract(IsOneWay = true)]
        void Receive(Message msg);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(Message msg);

        [OperationContract(IsOneWay = true)]
        void SessionInitMGSettings(Session session);

        [OperationContract(IsOneWay = true)]
        void ReceivePerchment(FileMessage fMsg);

        [OperationContract(IsOneWay = true)]
        void ReceiveHero(Hero hero);

        [OperationContract(IsOneWay = true)]
        void ReciveHeroEyes(HeroDetails_Eyes heroEyes);

        [OperationContract(IsOneWay = true)]
        void ReciveHeroOccupationByRace(AllHeroOccupations Occupations);

        [OperationContract(IsOneWay = true)]
        void ReciveOccupationInfo(OccupationInfo info);

        [OperationContract(IsOneWay = true)]
        void HeroRegistrationPartOne(ServerMessage msg);

        [OperationContract(IsOneWay = true)]
        void ReciveSkillsAndAbilities(OccupationAndRaceInfo info);

        [OperationContract(IsOneWay = true)]
        void ReciveAbilityNames(AbilityNames abNames);

        [OperationContract(IsOneWay = true)]
        void ReciveSkillNames(SkillNames skNames);

        [OperationContract(IsOneWay = true)]
        void ReciveFullAbilityInfo(FullAbilityInfo abInfo);

        [OperationContract(IsOneWay = true)]
        void ReciveFullSkillInfo(FullSkillInfo skInfo);

        [OperationContract(IsOneWay = true)]
        void ReciveFullHeroChart(HeroFullChart chart);

        [OperationContract(IsOneWay = true)]
        void ReciveRaceAbilityNames(AbilityNames abNames);

        [OperationContract(IsOneWay = true)]
        void ReciveOccupationAbilityNames(AbilityNames abNames);

        [OperationContract(IsOneWay = true)]
        void ReciveRaceSkillNames(SkillNames skNames);

        [OperationContract(IsOneWay = true)]
        void ReciveOccupationSkillNames(SkillNames skNames);

        [OperationContract(IsOneWay = true)]
        void ReciveIfHeroCreated(HeroStatus status);
    }
}
