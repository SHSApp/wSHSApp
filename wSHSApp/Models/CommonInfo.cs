using System.Collections.Generic;

namespace wSHSApp.Models
{
    public class CommonInfo
    {
        /// <summary>
        /// Флаг указывающий пустой объект или нет (загрузились ли данные)
        /// </summary>
        public bool IsNotEmpty { get; set; }
        /// <summary>
        /// Путь к фотографии (вид спереди)
        /// </summary>
        public string? PathToFotoFas { get; set; }
        /// <summary>
        /// Путь к фотографии (вид сбоку)
        /// </summary>
        public string? PathToFotoProf { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string? Surname { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string? Lastname { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public string? Birthday { get; set; }
        /// <summary>
        /// Гражданство
        /// </summary>
        public string? Citizenship { get; set; }
        /// <summary>
        /// Национальность
        /// </summary>
        public string? Nation { get; set; }
        /// <summary>
        /// Каким судом осужден
        /// </summary>
        public string? CourtName { get; set; }
        /// <summary>
        /// Дата осуждения
        /// </summary>
        public string? СondemnationDate { get; set; }
        /// <summary>
        /// Дата вступления приговора в законную силу
        /// </summary>
        public string? VerdictDate { get; set; }
        /// <summary>
        /// Статьи осуждения
        /// </summary>
        public string? Articles { get; set; }
        /// <summary>
        /// Срок наказания
        /// </summary>
        public string? Term { get; set; }
        /// <summary>
        /// Начало срока
        /// </summary>
        public string? BeginTerm { get; set; }
        /// <summary>
        /// Конец срока
        /// </summary>
        public string? EndTerm { get; set; }
        /// <summary>
        /// Дата заключения под стражу
        /// </summary>
        public string? DetentionDate { get; set; }
        /// <summary>
        /// Дата прибытия в учреждение
        /// </summary>
        public string? ArrivalDate { get; set; }
        /// <summary>
        /// Из какого учреждения прибыл
        /// </summary>
        public string? ArrivalInstitution { get; set; }
        /// <summary>
        /// Из какого территориального органа
        /// </summary>
        public string? ArrivalRegion { get; set; }
        /// <summary>
        /// Табельный номер
        /// </summary>
        public string? PersonalNumber { get; set; }
        /// <summary>
        /// Номер отряда
        /// </summary>
        public string? GroupId { get; set; }
        /// <summary>
        /// Условия отбывания наказания
        /// </summary>
        public string? Conditions { get; set; }
        /// <summary>
        /// Часть срока для УДО
        /// </summary>
        public string? PartUDO { get; set; }
        /// <summary>
        /// Дата УДО
        /// </summary>
        public string? DateUDO { get; set; }
        /// <summary>
        /// Часть срока для замены неотбытой части наказания
        /// </summary>
        public string? PartZMN { get; set; }
        /// <summary>
        /// Дата замены неотбытой части наказания
        /// </summary>
        public string? DateZMN { get; set; }
        /// <summary>
        /// Часть срока для колонии-поселения
        /// </summary>
        public string? PartKP { get; set; }
        /// <summary>
        /// Дата колонии-поселения
        /// </summary>
        public string? DateKP { get; set; }
        /// <summary>
        /// Часть срока для замены принудительными работами
        /// </summary>
        public string? PartPR { get; set; }
        /// <summary>
        /// Дата для замены принудительными работами
        /// </summary>
        public string? DatePR { get; set; }
        /// <summary>
        /// Содержит ли расширенную информацию
        /// </summary>
        public bool IsExtraData { get; set; }
        /// <summary>
        /// Место рождения
        /// </summary>
        public string? PlaceOfBirth { get; set; }
        /// <summary>
        /// Место работы
        /// </summary>
        public string? PlaceOfWork { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        public string? JobTitle { get; set; }
        /// <summary>
        /// Место регистрации
        /// </summary>
        public string? PlaceOfRegistration { get; set; }        
        /// <summary>
        /// Образование
        /// </summary>
        public string? Education { get; set; }
        /// <summary>
        /// Семейное положение
        /// </summary>
        public string? MaritalStatus { get; set; }
        /// <summary>
        /// Общий статус (убыл, освободился, умер, в наличии)
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Список условия отбывания
        /// </summary>
        public List<ConditionItem>? ConditionList { get; set; }
        /// <summary>
        /// Список движения по отрядам
        /// </summary>
        public List<MovingItem>? MovingList { get; set; }
        /// <summary>
        /// Информация об освобождении
        /// </summary>
        public Release? Release { get; set; }
    }
}
