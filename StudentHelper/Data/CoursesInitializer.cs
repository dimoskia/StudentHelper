using System.Collections.Generic;
using System.Data.Entity;
using StudentHelper.Models;
using System;

namespace StudentHelper.Data
{
    public class CoursesInitializer : DropCreateDatabaseAlways<StudentHelperContext>
    {
        protected override void Seed(StudentHelperContext context)
        {
            var courses = new List<Course>
            {
                new Course { Description = "Развој на напредни серверски базирани веб апликации базирани на шаблони. Развој на веб апликации во облак.", Title = "Веб програмирање", Type = "изборен", Program = "ПЕТ", Year = 3, Semester = "Летен" },
                new Course { Description = "Да се воведат студентите во парадигмата на генеричкото програмирање. Да се запознаат со апстрактни податочни типови, креирање на темплејт класи и функции.", Title = "Напредно програмирање", Type = "изборен", Program = "КНИ", Year = 2, Semester = "Зимски" },
                new Course { Description = "Целта на предметот е да го запознае студентот со основните концепти на објектно-ориентираното програмирање. За таа цел ќе бидат воведени концептите на објекти и класи, eнкапсулација, наследување и полиморфизам.", Title = "Објекто-ориентирано програмирање", Type = "задолжителен", Program = "КНИ", Year = 1, Semester = "Зимски" },
                new Course { Description = "Да се воведат студентите во парадигмата на структурното програмирање, да го разберат концептот на алгоритми и да се оспособат да развиваат алгоритми, да кодират, тестираат и компајлираат програми.", Title = "Структурно програмирање", Type = "задолжителен", Program = "КНИ", Year = 1, Semester = "Зимски" },
                new Course { Description = "Со завршување на овој курс се очекува студентите да се запознаени со техниките за дизајнирање на богати графички кориснички интерфејси и откривање и отстранување на грешки во истата.", Title = "Визуелно програмирање", Type = "изборен", Program = "ПЕТ", Year = 2, Semester = "Летен" },
                new Course { Description = "Целта на курсот е да овозможи запознавање на студентите со основните на програмирањето на интернет клиентската страна. Во таа насока студентите ќе бидат запознаени со дел од програмските јазици и технологиите за пишување програми што се извршуваат на клиентската страна. ", Title = "Интернет програмирање", Type = "изборен", Program = "КНИ", Year = 2, Semester = "Летен" },
                new Course { Description = "Познавање на механизмите кај HTTP протоколот. Запознавање со платформи за развој на интернет апликации. Креирање и развој на веб апликции. Креирање и користење на веб сервиси. ", Title = "Интернет технологии", Type = "изборен", Program = "КНИ", Year = 2, Semester = "Зимски" },
                new Course { Description = "Предметот е од подршка и во него се воведуваат поимите за интеграл кај функции од една променлива, функции од повеќе променливи, парцијални изводи и двојни интеграли", Title = "Калкулус 2", Type = "задолжителен", Program = "ПЕТ", Year = 1, Semester = "Зимски" },
                new Course { Description = "Да се запознае студентот со основните елементи од дискретната математика која е основа на информатичката технологија. Во тој контекст да научи да ги применува формалните методи на симболичката исказна и предикатна логика", Title = "Дискретна математика 1", Type = "задолжителен", Program = "КНИ", Year = 1, Semester = "Летен" },
                new Course { Description = "Да се воведат основните концепти од веројатност и статистичко анализа со дискусија на примените во компјутерските науки. ", Title = "Веројатност и статистика", Type = "задолжителен", Program = "КНИ", Year = 2, Semester = "Зимски" },
                new Course { Description = "Солидно познавање на основите на информатичко-комуникасицките технологии, нивното создавање, моментална состојба и иднината; начинот на кој функционираат сметачите, основите на Веб, обработката на сликите, видеото и анимациите", Title = "Вовед во информатика", Type = "задолжителен", Program = "КНИ", Year = 1, Semester = "Летен" },
                new Course { Description = "Целта на курсот е да студентите да се стекнат со вештини потребни за академско и техничко пишување и презентирање, со водење сметка за етиката и критичкото мислење. ", Title = "Професионални вештини", Type = "задолжителен", Program = "ПЕТ", Year = 1, Semester = "Зимски" },
                new Course { Description = "По комплетирање на курсот кандидатите се очекува да бидат запознаени со основите на структурата и начинот на работа на глобалната мрежа и да добијат воведни знаења за можностите и начините на користење на основните интернет сервиси", Title = "Вовед во интернет", Type = "задолжителен", Program = "КНИ", Year = 1, Semester = "Зимски" },
                new Course { Description = "По комплетирање на курсот кандидатите се очекува студентите за знаат да развиваат напредни веб страници со современ дизајн, употреба на ХТМЛ и каскадни стилови", Title = "Напреден веб дизајн", Type = "изборен", Program = "ПЕТ", Year = 4, Semester = "Зимски" },
                new Course { Description = "Целта на предметот е студентитеда се стекнат со основните теоретски и практични знаења за алгоритмите за обработка на природните јазици.", Title = "Обработка на природните јазици", Type = "изборен", Program = "КНИ", Year = 4, Semester = "Летен" },
                new Course { Description = "Запознавање на студентите со напредните концепти на релационите бази на податоци, неопходни при креирање, управување и одржување на базите на податоци, како и за развој на податочно ориентирани системи", Title = "Напредни бази на податоци", Type = "изборен", Program = "ПЕТ", Year = 4, Semester = "Летен" },
                new Course { Description = "Запознавање со oрганизацијата и манипулацијата со податоците организирани во складови на податоци, како и основните операции и алгоритми за работа со складови на податоци", Title = "Складови на податоци и аналитичка обработка", Type = "изборен", Program = "КНИ", Year = 4, Semester = "Зимски" },
                new Course { Description = "Целта на курсот е студентите да се запознаат со процесите и технологијата на еВлада.По завршувањето на курсот кандидатите: ќе имаат продлабочени знаења за организацијата и процесите на испорака на информации и сервиси со користење на еТехнологии на граѓаните и правните субјекти во општеството", Title = "е-Влада", Type = "изборен", Program = "КНИ", Year = 2, Semester = "Зимски" },
                new Course { Description = "Запознавање на студентот со основните концепти за работа со бази на податоци, начините на нивно моделирање и имплементирање, како и работа со прашалните јазици", Title = "Бази на податоци", Type = "задолжителен", Program = "КНИ", Year = 3, Semester = "Летен" },
                new Course { Description = "Студентите да се запознаат со основните на Буловите алгебри и нивната улога и примена во информатичките технологии. Да ги совладаат основните техники на броење и истите да ги применува при решавање на практични задачи", Title = "Дискретна математика 2", Type = "задолжителен", Program = "ПЕТ", Year = 1, Semester = "Зимски" },
            };

            

            var staffs = new List<Staff>
            {
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/verica-bakeva", FirstName = "Верица", LastName = "Бакева", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/marjan-gushev", FirstName = "Марјан", LastName = "Гушев", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/katerina-zdravkova", FirstName = "Катерина", LastName = "Здравкова", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/suzana-loshkovska", FirstName = "Сузана", LastName = "Лошковска", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/zaneta-popeska", FirstName = "Жанета", LastName = "Попеска", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/vladimir-trajkovik", FirstName = "Владимир", LastName = "Трајковиќ", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/marija-mihova", FirstName = "Марија", LastName = "Михова", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/slobodan-kalajdziski", FirstName = "Слободан", LastName = "Калајџиски", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/nevena-ackovska", FirstName = "Невена", LastName = "Ацковска", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/magdalena-kostoska", FirstName = "Магдалена", LastName = "Костоска", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/vesna-kirandziska", FirstName = "Весна", LastName = "Киранџиска", Title = "м-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/ilinka-ivanoska", FirstName = "Илинка", LastName = "Иваноска", Title = "м-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/emil-stankov", FirstName = "Емил", LastName = "Станков", Title = "м-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/staff/riste-stojanov", FirstName = "Ристе", LastName = "Стојанов", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/content/%D0%B4-%D1%80-%D0%B1%D0%BE%D1%98%D0%B0%D0%BD%D0%B0-%D0%BA%D0%BE%D1%82%D0%B5%D1%81%D0%BA%D0%B0", FirstName = "Бојана", LastName = "Котеска", Title = "д-р" },
                new Staff { DetailsUrl = "https://www.finki.ukim.mk/mk/content/%D0%BC-%D1%80-%D0%B0%D0%BB%D0%B5%D0%BA%D1%81%D0%B0%D0%BD%D0%B4%D0%B0%D1%80-%D1%81%D1%82%D0%BE%D1%98%D0%BC%D0%B5%D0%BD%D1%81%D0%BA%D0%B8", FirstName = "Александар", LastName = "Стојменски", Title = "м-р" }
            };

            courses.ForEach(course => context.Courses.Add(course));
            staffs.ForEach(staff => context.Staffs.Add(staff));

            base.Seed(context);
        }
    }
}