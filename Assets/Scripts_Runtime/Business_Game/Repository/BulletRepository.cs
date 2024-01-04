using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit.Business.Game {

    public class BulletRepository {

        Dictionary<int, BulletEntity> all;
        BulletEntity selectedBullet;

        BulletEntity[] temp;

        public BulletRepository() {
            all = new Dictionary<int, BulletEntity>();
            temp = new BulletEntity[1000];
        }

        public void Bullet_Add(BulletEntity bullet) {
            all.Add(bullet.entityID, bullet);
        }

        public bool Bullet_TryGet(int entityID, out BulletEntity bullet) {
            return all.TryGetValue(entityID, out bullet);
        }

        public void SelectedBullet_Set(BulletEntity bullet) {
            selectedBullet = bullet;
        }

        public BulletEntity SelectedBullet_Get() {
            return selectedBullet;
        }

        public bool Bullet_TryGetAlive(int entityID, out BulletEntity bullet) {
            bool has = all.TryGetValue(entityID, out bullet);
            return has && !bullet.isDead;
        }

        public void Bullet_Remove(BulletEntity bullet) {
            all.Remove(bullet.entityID);
        }

    }

}