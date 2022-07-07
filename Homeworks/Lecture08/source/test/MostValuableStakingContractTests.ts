import { expect } from "chai";
import { ethers } from "hardhat";
import { SignerWithAddress } from "@nomiclabs/hardhat-ethers/signers";

import { MostValuableToken, MostValuableRewardToken, MostValuableStakingContract, ERC20 } from "../typechain";
import { constants } from "../constants";

describe("MostValuableStakingContract", function () {

    let mvt: MostValuableToken;
    let mvrt: MostValuableRewardToken;
    let mvsc: MostValuableStakingContract;

    let admin: SignerWithAddress;
    let addr1: SignerWithAddress;
    let addr2: SignerWithAddress;
    
    beforeEach(async () => {
      [admin, addr1, addr2] = await ethers.getSigners();

      const MostValuableToken = await ethers.getContractFactory("MostValuableToken");
      mvt = await MostValuableToken.deploy(constants.MostValuableToken.TOKEN_SUPPLY);
      await mvt.deployed();

      await mvt.transfer(addr1.address, 1000);
      await mvt.transfer(addr2.address, 1000);

      const MostValuableRewardToken = await ethers.getContractFactory("MostValuableRewardToken");
      mvrt = await MostValuableRewardToken.deploy();
      await mvrt.deployed();

      const MostValuableStakingContract = await ethers.getContractFactory("MostValuableStakingContract");
      mvsc = await MostValuableStakingContract.deploy(mvt.address, mvrt.address);
      await mvsc.deployed();

      await mvrt.transferOwnership(mvsc.address);        
    })
  
    // it("Should return proper staling and reward token addresses", async function () {
    //   expect(await (mvsc.StakingToken as ERC20).()).to.equal(mvt.address);
    //   expect(await mvsc.RewardTokenAddress()).to.equal(mvrt.address);
      
    // });
  
    it("User should be able to stake MVTs", async function () {
      await mvt.connect(addr1).approve(mvsc.address, 10);
      await mvsc.connect(addr1).stake(10);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(10);
    });

    it("User should be able to withdraw MVTs", async function () {
      let startingMVTAmount = await mvt.balanceOf(addr1.address);
      await mvt.connect(addr1).approve(mvsc.address, 10);
      await mvsc.connect(addr1).stake(10);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(10);

      await mvsc.connect(addr1).withdraw();

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(0);

      let endingMVTAmount = await mvt.balanceOf(addr1.address);

      expect(startingMVTAmount).to.equal(endingMVTAmount);
    });

    it("User should be able to claim rewards MVRTs without withdrawing staked MVTs", async function () {

      expect(await mvrt.balanceOf(addr1.address))
        .to.equal(0);

      let stakedMVTAmount = 10;
      await mvt.connect(addr1).approve(mvsc.address, stakedMVTAmount);
      await mvsc.connect(addr1).stake(stakedMVTAmount);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(stakedMVTAmount);

      let elapsedSeconds = 60;
      await ethers.provider.send('evm_increaseTime', [elapsedSeconds]);
      await mvsc.connect(addr1).claimRewards();

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(stakedMVTAmount);

        expect(await mvrt.balanceOf(addr1.address))
        .to.equal(stakedMVTAmount * elapsedSeconds);
    });

    it("User should receive rewards MVRTs when withdrawing staked MVTs", async function () {

      expect(await mvrt.balanceOf(addr1.address))
        .to.equal(0);

      let stakedMVTAmount = 10;
      await mvt.connect(addr1).approve(mvsc.address, stakedMVTAmount);
      await mvsc.connect(addr1).stake(stakedMVTAmount);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(stakedMVTAmount);

      let elapsedSeconds = 60;
      await ethers.provider.send('evm_increaseTime', [elapsedSeconds]);
      await mvsc.connect(addr1).withdraw();

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(0);

      expect(await mvrt.balanceOf(addr1.address))
        .to.equal(stakedMVTAmount * elapsedSeconds);
    });

    it("User should see correct rewards amount", async function () {

      let stakedMVTAmount = 10;
      await mvt.connect(addr1).approve(mvsc.address, stakedMVTAmount);
      await mvsc.connect(addr1).stake(stakedMVTAmount);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(stakedMVTAmount);

      let elapsedSeconds = 60;
      await ethers.provider.send('evm_increaseTime', [elapsedSeconds]);
      await mvt.connect(addr1).approve(mvsc.address, stakedMVTAmount);

      expect(await mvsc.connect(addr1).calculateRewards(addr1.address))
        .to.equal(stakedMVTAmount * elapsedSeconds);

      expect(await mvsc.checkStake(addr1.address))
          .to.equal(stakedMVTAmount);
    });

    it("Staking again should claim rewards MVRTs and increase the amount of staked MVTs", async function () {
      expect(await mvrt.balanceOf(addr1.address))
        .to.equal(0);

      let stakedMVTAmount = 10;
      await mvt.connect(addr1).approve(mvsc.address, stakedMVTAmount * 2);
      await mvsc.connect(addr1).stake(stakedMVTAmount);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(stakedMVTAmount);

      let elapsedSeconds = 60;
      await ethers.provider.send('evm_increaseTime', [elapsedSeconds]);

      await mvsc.connect(addr1).stake(stakedMVTAmount);

      expect(await mvrt.balanceOf(addr1.address))
        .to.equal(stakedMVTAmount * elapsedSeconds);

      expect(await mvsc.checkStake(addr1.address))
        .to.equal(stakedMVTAmount * 2);
      
      expect(await mvsc.connect(addr1).calculateRewards(addr1.address))
        .to.equal(0);
    });
});