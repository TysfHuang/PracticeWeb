﻿namespace Practice.Domain.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Practice.Domain.Concrete;
    using Practice.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Practice.Domain.Concrete.AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Practice.Domain.Concrete.AppIdentityDbContext";
        }

        protected override void Seed(Practice.Domain.Concrete.AppIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            //SetDefaultStoreInfo(context);
            //SetDefaultCityAndCountryInfo(context);
            //SetDefaultAccountAndRole(context);
        }

        private void SetDefaultAccountAndRole(Practice.Domain.Concrete.AppIdentityDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "";
            string password = "";
            string email = "@example.com";
            int CityID = 1;
            int CountryID = 1;

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }

            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email, CityID = CityID, CountryID = CountryID },
                    password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            context.SaveChanges();
        }

        public void SetDefaultStoreInfo(AppIdentityDbContext context)
        {
            var brands = new List<Brand>
                {
                    new Brand { Name = "SanDisk"},
                    new Brand { Name = "Micron"},
                    new Brand { Name = "Samsung"},
                    new Brand { Name = "Seagate"},
                    new Brand { Name = "WD"},
                    new Brand { Name = "MSI"},
                    new Brand { Name = "ASUS"},
                    new Brand { Name = "GIGABYTE"},
                    new Brand { Name = "EVGA"},
                    new Brand { Name = "Google"},
                    new Brand { Name = "Intel"},
                    new Brand { Name = "AMD"},
                };
            brands.ForEach(s => context.Brands.AddOrUpdate(s));
            context.SaveChanges();

            var categories = new List<Category>
                {
                    new Category { Name = "Storage" },
                    new Category { Name = "VideoCard" },
                    new Category { Name = "SmartPhone" },
                    new Category { Name = "CPU" }
                };
            categories.ForEach(s => context.Categories.AddOrUpdate(s));
            context.SaveChanges();
        }

        public void SetDefaultCityAndCountryInfo(AppIdentityDbContext context)
        {
            var cities = new List<City>
            {
                new City{Name = "臺北市"},
                new City{Name = "臺中市"},
                new City{Name = "基隆市"},
                new City{Name = "臺南市"},
                new City{Name = "高雄市"},
                new City{Name = "新北市"},
                new City{Name = "宜蘭縣"},
                new City{Name = "桃園市"},
                new City{Name = "嘉義市"},
                new City{Name = "新竹縣"},
                new City{Name = "苗栗縣"},
                new City{Name = "南投縣"},
                new City{Name = "彰化縣"},
                new City{Name = "新竹市"},
                new City{Name = "雲林縣"},
                new City{Name = "嘉義縣"},
                new City{Name = "屏東縣"},
                new City{Name = "花蓮縣"},
                new City{Name = "臺東縣"},
                new City{Name = "金門縣"},
                new City{Name = "澎湖縣"},
                new City{Name = "連江縣"}
            };
            cities.ForEach(c => context.Cities.AddOrUpdate(c));
            context.SaveChanges();

            var countries = new List<Country>
            {
                new Country{CityID = 1, Name = "松山區"},
                new Country{CityID = 1, Name = "大安區"},
                new Country{CityID = 1, Name = "中正區"},
                new Country{CityID = 1, Name = "中正區"},
                new Country{CityID = 1, Name = "萬華區"},
                new Country{CityID = 1, Name = "大同區"},
                new Country{CityID = 1, Name = "中山區"},
                new Country{CityID = 1, Name = "文山區"},
                new Country{CityID = 1, Name = "南港區"},
                new Country{CityID = 1, Name = "內湖區"},
                new Country{CityID = 1, Name = "士林區"},
                new Country{CityID = 1, Name = "北投區"},
                new Country{CityID = 1, Name = "信義區"},
                new Country{CityID = 2, Name = "中區"},
                new Country{CityID = 2, Name = "東區"},
                new Country{CityID = 2, Name = "南區"},
                new Country{CityID = 2, Name = "西區"},
                new Country{CityID = 2, Name = "北區"},
                new Country{CityID = 2, Name = "西屯區"},
                new Country{CityID = 2, Name = "南屯區"},
                new Country{CityID = 2, Name = "北屯區"},
                new Country{CityID = 2, Name = "豐原區"},
                new Country{CityID = 2, Name = "東勢區"},
                new Country{CityID = 2, Name = "大甲區"},
                new Country{CityID = 2, Name = "清水區"},
                new Country{CityID = 2, Name = "沙鹿區"},
                new Country{CityID = 2, Name = "梧棲區"},
                new Country{CityID = 2, Name = "后里區"},
                new Country{CityID = 2, Name = "神岡區"},
                new Country{CityID = 2, Name = "潭子區"},
                new Country{CityID = 2, Name = "大雅區"},
                new Country{CityID = 2, Name = "新社區"},
                new Country{CityID = 2, Name = "石岡區"},
                new Country{CityID = 2, Name = "外埔區"},
                new Country{CityID = 2, Name = "大安區"},
                new Country{CityID = 2, Name = "烏日區"},
                new Country{CityID = 2, Name = "大肚區"},
                new Country{CityID = 2, Name = "龍井區"},
                new Country{CityID = 2, Name = "霧峰區"},
                new Country{CityID = 2, Name = "太平區"},
                new Country{CityID = 2, Name = "大里區"},
                new Country{CityID = 2, Name = "和平區"},
                new Country{CityID = 3, Name = "中正區"},
                new Country{CityID = 3, Name = "七堵區"},
                new Country{CityID = 3, Name = "暖暖區"},
                new Country{CityID = 3, Name = "仁愛區"},
                new Country{CityID = 3, Name = "中山區"},
                new Country{CityID = 3, Name = "安樂區"},
                new Country{CityID = 3, Name = "信義區"},
                new Country{CityID = 4, Name = "東區"},
                new Country{CityID = 4, Name = "南區"},
                new Country{CityID = 4, Name = "北區"},
                new Country{CityID = 4, Name = "安南區"},
                new Country{CityID = 4, Name = "安平區"},
                new Country{CityID = 4, Name = "中西區"},
                new Country{CityID = 4, Name = "新營區"},
                new Country{CityID = 4, Name = "鹽水區"},
                new Country{CityID = 4, Name = "柳營區"},
                new Country{CityID = 4, Name = "白河區"},
                new Country{CityID = 4, Name = "後壁區"},
                new Country{CityID = 4, Name = "東山區"},
                new Country{CityID = 4, Name = "麻豆區"},
                new Country{CityID = 4, Name = "下營區"},
                new Country{CityID = 4, Name = "六甲區"},
                new Country{CityID = 4, Name = "官田區"},
                new Country{CityID = 4, Name = "大內區"},
                new Country{CityID = 4, Name = "佳里區"},
                new Country{CityID = 4, Name = "西港區"},
                new Country{CityID = 4, Name = "七股區"},
                new Country{CityID = 4, Name = "將軍區"},
                new Country{CityID = 4, Name = "北門區"},
                new Country{CityID = 4, Name = "學甲區"},
                new Country{CityID = 4, Name = "新化區"},
                new Country{CityID = 4, Name = "善化區"},
                new Country{CityID = 4, Name = "新市區"},
                new Country{CityID = 4, Name = "安定區"},
                new Country{CityID = 4, Name = "山上區"},
                new Country{CityID = 4, Name = "左鎮區"},
                new Country{CityID = 4, Name = "仁德區"},
                new Country{CityID = 4, Name = "歸仁區"},
                new Country{CityID = 4, Name = "關廟區"},
                new Country{CityID = 4, Name = "龍崎區"},
                new Country{CityID = 4, Name = "玉井區"},
                new Country{CityID = 4, Name = "楠西區"},
                new Country{CityID = 4, Name = "南化區"},
                new Country{CityID = 4, Name = "永康區"},
                new Country{CityID = 5, Name = "鹽埕區"},
                new Country{CityID = 5, Name = "鼓山區"},
                new Country{CityID = 5, Name = "左營區"},
                new Country{CityID = 5, Name = "楠梓區"},
                new Country{CityID = 5, Name = "三民區"},
                new Country{CityID = 5, Name = "新興區"},
                new Country{CityID = 5, Name = "前金區"},
                new Country{CityID = 5, Name = "苓雅區"},
                new Country{CityID = 5, Name = "前鎮區"},
                new Country{CityID = 5, Name = "旗津區"},
                new Country{CityID = 5, Name = "小港區"},
                new Country{CityID = 5, Name = "鳳山區"},
                new Country{CityID = 5, Name = "林園區"},
                new Country{CityID = 5, Name = "大寮區"},
                new Country{CityID = 5, Name = "大樹區"},
                new Country{CityID = 5, Name = "大社區"},
                new Country{CityID = 5, Name = "仁武區"},
                new Country{CityID = 5, Name = "鳥松區"},
                new Country{CityID = 5, Name = "岡山區"},
                new Country{CityID = 5, Name = "橋頭區"},
                new Country{CityID = 5, Name = "燕巢區"},
                new Country{CityID = 5, Name = "田寮區"},
                new Country{CityID = 5, Name = "阿蓮區"},
                new Country{CityID = 5, Name = "路竹區"},
                new Country{CityID = 5, Name = "湖內區"},
                new Country{CityID = 5, Name = "茄萣區"},
                new Country{CityID = 5, Name = "永安區"},
                new Country{CityID = 5, Name = "彌陀區"},
                new Country{CityID = 5, Name = "梓官區"},
                new Country{CityID = 5, Name = "旗山區"},
                new Country{CityID = 5, Name = "美濃區"},
                new Country{CityID = 5, Name = "六龜區"},
                new Country{CityID = 5, Name = "甲仙區"},
                new Country{CityID = 5, Name = "杉林區"},
                new Country{CityID = 5, Name = "內門區"},
                new Country{CityID = 5, Name = "茂林區"},
                new Country{CityID = 5, Name = "桃源區"},
                new Country{CityID = 5, Name = "那瑪夏區"},
                new Country{CityID = 6, Name = "新莊區"},
                new Country{CityID = 6, Name = "林口區"},
                new Country{CityID = 6, Name = "五股區"},
                new Country{CityID = 6, Name = "蘆洲區"},
                new Country{CityID = 6, Name = "三重區"},
                new Country{CityID = 6, Name = "泰山區"},
                new Country{CityID = 6, Name = "新店區"},
                new Country{CityID = 6, Name = "石碇區"},
                new Country{CityID = 6, Name = "深坑區"},
                new Country{CityID = 6, Name = "坪林區"},
                new Country{CityID = 6, Name = "烏來區"},
                new Country{CityID = 6, Name = "板橋區"},
                new Country{CityID = 6, Name = "三峽區"},
                new Country{CityID = 6, Name = "鶯歌區"},
                new Country{CityID = 6, Name = "樹林區"},
                new Country{CityID = 6, Name = "中和區"},
                new Country{CityID = 6, Name = "土城區"},
                new Country{CityID = 6, Name = "瑞芳區"},
                new Country{CityID = 6, Name = "平溪區"},
                new Country{CityID = 6, Name = "雙溪區"},
                new Country{CityID = 6, Name = "貢寮區"},
                new Country{CityID = 6, Name = "金山區"},
                new Country{CityID = 6, Name = "萬里區"},
                new Country{CityID = 6, Name = "淡水區"},
                new Country{CityID = 6, Name = "汐止區"},
                new Country{CityID = 6, Name = "三芝區"},
                new Country{CityID = 6, Name = "石門區"},
                new Country{CityID = 6, Name = "八里區"},
                new Country{CityID = 6, Name = "永和區"},
                new Country{CityID = 7, Name = "宜蘭市"},
                new Country{CityID = 7, Name = "頭城鎮"},
                new Country{CityID = 7, Name = "礁溪鄉"},
                new Country{CityID = 7, Name = "壯圍鄉"},
                new Country{CityID = 7, Name = "員山鄉"},
                new Country{CityID = 7, Name = "羅東鎮"},
                new Country{CityID = 7, Name = "五結鄉"},
                new Country{CityID = 7, Name = "冬山鄉"},
                new Country{CityID = 7, Name = "蘇澳鎮"},
                new Country{CityID = 7, Name = "三星鄉"},
                new Country{CityID = 7, Name = "大同鄉"},
                new Country{CityID = 7, Name = "南澳鄉"},
                new Country{CityID = 8, Name = "桃園區"},
                new Country{CityID = 8, Name = "大溪區"},
                new Country{CityID = 8, Name = "中壢區"},
                new Country{CityID = 8, Name = "楊梅區"},
                new Country{CityID = 8, Name = "蘆竹區"},
                new Country{CityID = 8, Name = "大園區"},
                new Country{CityID = 8, Name = "龜山區"},
                new Country{CityID = 8, Name = "八德區"},
                new Country{CityID = 8, Name = "龍潭區"},
                new Country{CityID = 8, Name = "平鎮區"},
                new Country{CityID = 8, Name = "新屋區"},
                new Country{CityID = 8, Name = "觀音區"},
                new Country{CityID = 8, Name = "復興區"},
                new Country{CityID = 9, Name = "東區"},
                new Country{CityID = 9, Name = "西區"},
                new Country{CityID = 10, Name = "竹東鎮"},
                new Country{CityID = 10, Name = "關西鎮"},
                new Country{CityID = 10, Name = "新埔鎮"},
                new Country{CityID = 10, Name = "竹北市"},
                new Country{CityID = 10, Name = "湖口鄉"},
                new Country{CityID = 10, Name = "橫山鄉"},
                new Country{CityID = 10, Name = "新豐鄉"},
                new Country{CityID = 10, Name = "芎林鄉"},
                new Country{CityID = 10, Name = "寶山鄉"},
                new Country{CityID = 10, Name = "北埔鄉"},
                new Country{CityID = 10, Name = "峨眉鄉"},
                new Country{CityID = 10, Name = "尖石鄉"},
                new Country{CityID = 10, Name = "五峰鄉"},
                new Country{CityID = 11, Name = "苗栗市"},
                new Country{CityID = 11, Name = "苑裡鎮"},
                new Country{CityID = 11, Name = "通霄鎮"},
                new Country{CityID = 11, Name = "公館鄉"},
                new Country{CityID = 11, Name = "銅鑼鄉"},
                new Country{CityID = 11, Name = "三義鄉"},
                new Country{CityID = 11, Name = "西湖鄉"},
                new Country{CityID = 11, Name = "頭屋鄉"},
                new Country{CityID = 11, Name = "竹南鎮"},
                new Country{CityID = 11, Name = "頭份市"},
                new Country{CityID = 11, Name = "造橋鄉"},
                new Country{CityID = 11, Name = "後龍鎮"},
                new Country{CityID = 11, Name = "三灣鄉"},
                new Country{CityID = 11, Name = "南庄鄉"},
                new Country{CityID = 11, Name = "大湖鄉"},
                new Country{CityID = 11, Name = "卓蘭鎮"},
                new Country{CityID = 11, Name = "獅潭鄉"},
                new Country{CityID = 11, Name = "泰安鄉"},
                new Country{CityID = 12, Name = "南投市"},
                new Country{CityID = 12, Name = "埔里鎮"},
                new Country{CityID = 12, Name = "草屯鎮"},
                new Country{CityID = 12, Name = "竹山鎮"},
                new Country{CityID = 12, Name = "集集鎮"},
                new Country{CityID = 12, Name = "名間鄉"},
                new Country{CityID = 12, Name = "鹿谷鄉"},
                new Country{CityID = 12, Name = "中寮鄉"},
                new Country{CityID = 12, Name = "魚池鄉"},
                new Country{CityID = 12, Name = "國姓鄉"},
                new Country{CityID = 12, Name = "水里鄉"},
                new Country{CityID = 12, Name = "信義鄉"},
                new Country{CityID = 12, Name = "仁愛鄉"},
                new Country{CityID = 13, Name = "彰化市"},
                new Country{CityID = 13, Name = "鹿港鎮"},
                new Country{CityID = 13, Name = "和美鎮"},
                new Country{CityID = 13, Name = "北斗鎮"},
                new Country{CityID = 13, Name = "員林市"},
                new Country{CityID = 13, Name = "溪湖鎮"},
                new Country{CityID = 13, Name = "田中鎮"},
                new Country{CityID = 13, Name = "二林鎮"},
                new Country{CityID = 13, Name = "線西鄉"},
                new Country{CityID = 13, Name = "伸港鄉"},
                new Country{CityID = 13, Name = "福興鄉"},
                new Country{CityID = 13, Name = "秀水鄉"},
                new Country{CityID = 13, Name = "花壇鄉"},
                new Country{CityID = 13, Name = "芬園鄉"},
                new Country{CityID = 13, Name = "大村鄉"},
                new Country{CityID = 13, Name = "埔鹽鄉"},
                new Country{CityID = 13, Name = "埔心鄉"},
                new Country{CityID = 13, Name = "永靖鄉"},
                new Country{CityID = 13, Name = "社頭鄉"},
                new Country{CityID = 13, Name = "二水鄉"},
                new Country{CityID = 13, Name = "田尾鄉"},
                new Country{CityID = 13, Name = "埤頭鄉"},
                new Country{CityID = 13, Name = "芳苑鄉"},
                new Country{CityID = 13, Name = "大城鄉"},
                new Country{CityID = 13, Name = "竹塘鄉"},
                new Country{CityID = 13, Name = "溪州鄉"},
                new Country{CityID = 14, Name = "東區"},
                new Country{CityID = 14, Name = "北區"},
                new Country{CityID = 14, Name = "香山區"},
                new Country{CityID = 15, Name = "斗六市"},
                new Country{CityID = 15, Name = "斗南鎮"},
                new Country{CityID = 15, Name = "虎尾鎮"},
                new Country{CityID = 15, Name = "西螺鎮"},
                new Country{CityID = 15, Name = "土庫鎮"},
                new Country{CityID = 15, Name = "北港鎮"},
                new Country{CityID = 15, Name = "古坑鄉"},
                new Country{CityID = 15, Name = "大埤鄉"},
                new Country{CityID = 15, Name = "莿桐鄉"},
                new Country{CityID = 15, Name = "林內鄉"},
                new Country{CityID = 15, Name = "二崙鄉"},
                new Country{CityID = 15, Name = "崙背鄉"},
                new Country{CityID = 15, Name = "麥寮鄉"},
                new Country{CityID = 15, Name = "東勢鄉"},
                new Country{CityID = 15, Name = "褒忠鄉"},
                new Country{CityID = 15, Name = "台西鄉"},
                new Country{CityID = 15, Name = "元長鄉"},
                new Country{CityID = 15, Name = "四湖鄉"},
                new Country{CityID = 15, Name = "口湖鄉"},
                new Country{CityID = 15, Name = "水林鄉"},
                new Country{CityID = 16, Name = "朴子市"},
                new Country{CityID = 16, Name = "布袋鎮"},
                new Country{CityID = 16, Name = "大林鎮"},
                new Country{CityID = 16, Name = "民雄鄉"},
                new Country{CityID = 16, Name = "溪口鄉"},
                new Country{CityID = 16, Name = "新港鄉"},
                new Country{CityID = 16, Name = "六腳鄉"},
                new Country{CityID = 16, Name = "東石鄉"},
                new Country{CityID = 16, Name = "義竹鄉"},
                new Country{CityID = 16, Name = "鹿草鄉"},
                new Country{CityID = 16, Name = "太保市"},
                new Country{CityID = 16, Name = "水上鄉"},
                new Country{CityID = 16, Name = "中埔鄉"},
                new Country{CityID = 16, Name = "竹崎鄉"},
                new Country{CityID = 16, Name = "梅山鄉"},
                new Country{CityID = 16, Name = "番路鄉"},
                new Country{CityID = 16, Name = "大埔鄉"},
                new Country{CityID = 16, Name = "阿里山鄉"},
                new Country{CityID = 17, Name = "屏東市"},
                new Country{CityID = 17, Name = "潮州鎮"},
                new Country{CityID = 17, Name = "東港鎮"},
                new Country{CityID = 17, Name = "恆春鎮"},
                new Country{CityID = 17, Name = "萬丹鄉"},
                new Country{CityID = 17, Name = "長治鄉"},
                new Country{CityID = 17, Name = "麟洛鄉"},
                new Country{CityID = 17, Name = "九如鄉"},
                new Country{CityID = 17, Name = "里港鄉"},
                new Country{CityID = 17, Name = "鹽埔鄉"},
                new Country{CityID = 17, Name = "高樹鄉"},
                new Country{CityID = 17, Name = "萬巒鄉"},
                new Country{CityID = 17, Name = "內埔鄉"},
                new Country{CityID = 17, Name = "竹田鄉"},
                new Country{CityID = 17, Name = "新埤鄉"},
                new Country{CityID = 17, Name = "枋寮鄉"},
                new Country{CityID = 17, Name = "新園鄉"},
                new Country{CityID = 17, Name = "崁頂鄉"},
                new Country{CityID = 17, Name = "林邊鄉"},
                new Country{CityID = 17, Name = "南州鄉"},
                new Country{CityID = 17, Name = "佳冬鄉"},
                new Country{CityID = 17, Name = "琉球鄉"},
                new Country{CityID = 17, Name = "車城鄉"},
                new Country{CityID = 17, Name = "滿州鄉"},
                new Country{CityID = 17, Name = "枋山鄉"},
                new Country{CityID = 17, Name = "三地門鄉"},
                new Country{CityID = 17, Name = "霧臺鄉"},
                new Country{CityID = 17, Name = "瑪家鄉"},
                new Country{CityID = 17, Name = "泰武鄉"},
                new Country{CityID = 17, Name = "來義鄉"},
                new Country{CityID = 17, Name = "春日鄉"},
                new Country{CityID = 17, Name = "獅子鄉"},
                new Country{CityID = 17, Name = "牡丹鄉"},
                new Country{CityID = 18, Name = "花蓮市"},
                new Country{CityID = 18, Name = "光復鄉"},
                new Country{CityID = 18, Name = "玉里鎮"},
                new Country{CityID = 18, Name = "新城鄉"},
                new Country{CityID = 18, Name = "吉安鄉"},
                new Country{CityID = 18, Name = "壽豐鄉"},
                new Country{CityID = 18, Name = "鳳林鎮"},
                new Country{CityID = 18, Name = "豐濱鄉"},
                new Country{CityID = 18, Name = "瑞穗鄉"},
                new Country{CityID = 18, Name = "富里鄉"},
                new Country{CityID = 18, Name = "秀林鄉"},
                new Country{CityID = 18, Name = "萬榮鄉"},
                new Country{CityID = 18, Name = "卓溪鄉"},
                new Country{CityID = 19, Name = "台東市"},
                new Country{CityID = 19, Name = "成功鎮"},
                new Country{CityID = 19, Name = "關山鎮"},
                new Country{CityID = 19, Name = "卑南鄉"},
                new Country{CityID = 19, Name = "大武鄉"},
                new Country{CityID = 19, Name = "太麻里鄉"},
                new Country{CityID = 19, Name = "東河鄉"},
                new Country{CityID = 19, Name = "長濱鄉"},
                new Country{CityID = 19, Name = "鹿野鄉"},
                new Country{CityID = 19, Name = "池上鄉"},
                new Country{CityID = 19, Name = "綠島鄉"},
                new Country{CityID = 19, Name = "延平鄉"},
                new Country{CityID = 19, Name = "海端鄉"},
                new Country{CityID = 19, Name = "達仁鄉"},
                new Country{CityID = 19, Name = "金峰鄉"},
                new Country{CityID = 19, Name = "蘭嶼鄉"},
                new Country{CityID = 20, Name = "金湖鎮"},
                new Country{CityID = 20, Name = "金沙鎮"},
                new Country{CityID = 20, Name = "金城鎮"},
                new Country{CityID = 20, Name = "金寧鄉"},
                new Country{CityID = 20, Name = "烈嶼鄉"},
                new Country{CityID = 20, Name = "烏坵鄉"},
                new Country{CityID = 21, Name = "馬公市"},
                new Country{CityID = 21, Name = "湖西鄉"},
                new Country{CityID = 21, Name = "白沙鄉"},
                new Country{CityID = 21, Name = "西嶼鄉"},
                new Country{CityID = 21, Name = "望安鄉"},
                new Country{CityID = 21, Name = "七美鄉"},
                new Country{CityID = 22, Name = "南竿鄉"},
                new Country{CityID = 22, Name = "北竿鄉"},
                new Country{CityID = 22, Name = "莒光鄉"},
                new Country{CityID = 22, Name = "東引鄉"}
            };
            countries.ForEach(c => context.Countries.AddOrUpdate(c));
            context.SaveChanges();
        }
    }
}
