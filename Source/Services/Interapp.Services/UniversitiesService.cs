﻿namespace Interapp.Services
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Repositories;
    using Interapp.Common.Enums;

    public class UniversitiesService : IUniversitiesService
    {
        private IRepository<University> universities;

        public UniversitiesService(IRepository<University> universities)
        {
            this.universities = universities;
        }

        public IQueryable<University> All()
        {
            return this.universities.All();
        }

        public void Create(string directorId, string name, int tuitionFee, int countryId)
        {
            var university = new University()
            {
                CountryId = countryId,
                DirectorId = directorId,
                Name = name,
                TuitionFee = tuitionFee
            };

            this.universities.Add(university);
            this.universities.SaveChanges();
        }

        public void DeleteById(int id)
        {
            this.universities.Delete(id);
            this.universities.SaveChanges();
        }

        public University GetById(int id)
        {
            return this.universities.All()
                .Where(u => u.Id == id)
                .Include(u => u.Director)
                .Include(u => u.Country)
                .Include(u => u.Students)
                .Include(u => u.InterestedStudents)
                .Include(u => u.DocumentRequirements)
                .FirstOrDefault();
        }

        public IQueryable<University> GetFiltered(FilterModel filter)
        {
            throw new NotImplementedException();
        }

        public void Update(
            int universityId,
            int countryId,
            string name,
            CambridgeLevel? cambridgeLevel,
            CambridgeResult? cambridgeScore,
            int? ibtToefl,
            int? pbtToefl,
            int? sat,
            int tuition)
        {
            var university = this.universities
                .All()
                .Where(u => u.Id == universityId)
                .FirstOrDefault();

            if (university != null)
            {
                university.CountryId = countryId;
                university.Name = name;
                university.RequiredCambridgeLevel = cambridgeLevel;
                university.RequiredCambridgeScore = cambridgeScore;
                university.RequiredIBTToefl = ibtToefl;
                university.RequiredPBTToefl = pbtToefl;
                university.RequiredSAT = sat;
                university.TuitionFee = tuition;

                this.universities.SaveChanges();
            }
        }

        public IQueryable<University> FilterUniversities(IQueryable<University> universities, FilterModel filter)
        {
            if (universities == null)
            {
                return universities;
            }

            universities = universities.OrderBy(u => u.Name);
            var page = 1;
            var pageSize = 10;

            if (filter != null)
            {
                if (filter.Filter != null)
                {
                    universities = universities.Where(u => u.Name.Contains(filter.Filter));
                }

                page = filter.Page;
                pageSize = filter.PageSize;

                if (filter.OrderBy == null)
                {
                    if (filter.Order == "asc")
                    {
                        if (filter.OrderBy == "name")
                        {
                            universities = universities.OrderBy(u => u.Name);
                        }
                        else if (filter.OrderBy == "country")
                        {
                            universities = universities.OrderBy(u => u.Country);
                        }
                        else if (filter.OrderBy == "tuition")
                        {
                            universities = universities.OrderBy(u => u.TuitionFee);
                        }
                        else if (filter.OrderBy == "sat")
                        {
                            universities = universities.OrderBy(u => u.RequiredSAT);
                        }
                        else if (filter.OrderBy == "toeflpbt")
                        {
                            universities = universities.OrderBy(u => u.RequiredPBTToefl);
                        }
                        else if (filter.OrderBy == "toeflibt")
                        {
                            universities = universities.OrderBy(u => u.RequiredIBTToefl);
                        }
                    }
                    else
                    {
                        if (filter.OrderBy == "name")
                        {
                            universities = universities.OrderByDescending(u => u.Name);
                        }
                        else if (filter.OrderBy == "country")
                        {
                            universities = universities.OrderByDescending(u => u.Country);
                        }
                        else if (filter.OrderBy == "tuition")
                        {
                            universities = universities.OrderByDescending(u => u.TuitionFee);
                        }
                        else if (filter.OrderBy == "sat")
                        {
                            universities = universities.OrderByDescending(u => u.RequiredSAT);
                        }
                        else if (filter.OrderBy == "toeflpbt")
                        {
                            universities = universities.OrderByDescending(u => u.RequiredPBTToefl);
                        }
                        else if (filter.OrderBy == "toeflibt")
                        {
                            universities = universities.OrderByDescending(u => u.RequiredIBTToefl);
                        }
                    }
                }
            }

            universities = universities
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return universities;
        }

        public IQueryable<University> AllExtended()
        {
            return this.universities
                .All()
                .Include(u => u.Country)
                .Include(u => u.Director)
                .Include(u => u.Director.Director);
        }

        public IQueryable<University> AllSimple()
        {
            return this.universities
                .All()
                .Include(u => u.Country);
        }
    }
}
