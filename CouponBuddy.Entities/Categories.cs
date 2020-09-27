using CouponBuddy.Entities.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public static class Categories
    {
        [CategoryInfo("Food", "Food & Restaurants", "food.jpg")]
        public const int FOOD = 1;
        [CategoryInfo("Beaches", "Beaches & Resorts", "beaches.jpg")]
        public const int BEACH = 2;
        [CategoryInfo("Activities", "Activities", "activities.jpg")]
        public const int ACTIVITIES = 3;
        [CategoryInfo("Entertainment", "Entertainment", "entertainment.jpg")]
        public const int ENTERTAINMENT = 4;

        [CategoryInfo("Specialties", "Gifts & Specialties", "Gifts.jpg")]
        public const int SPECIALTIES = 5;
        [CategoryInfo("Boutiques", "Boutiques", "Boutique.jpg")]
        public const int BOUTIQUES = 6;
        [CategoryInfo("Services", "Services", "Marina.jpg")]
        public const int SERVICES = 7;
        [CategoryInfo("Water", "Water & Boating", "Marina.jpg")]
        public const int WATER_AND_BOATS = 8;

        public static CategoryInfo GetCategory(int category)
        {
            var fields = typeof(Categories).GetFields();
            foreach (var field in fields)
            {
                int fieldValue = (int)field.GetValue(typeof(Categories));
                if (fieldValue == category)
                {
                    CategoryInfo info = (CategoryInfo)Attribute.GetCustomAttribute(field, typeof(CategoryInfo));
                    return info;
                }
            }
            return new CategoryInfo("No Category", "No Category", "No Category");
        }
    }
}